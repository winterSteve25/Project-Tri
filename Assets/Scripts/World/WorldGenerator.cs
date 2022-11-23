using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using SaveLoad;
using SaveLoad.Interfaces;
using UnityEngine;
using Utils;
using Utils.Data;
using World.Nodes;
using Debug = UnityEngine.Debug;

namespace World
{
    /// <summary>
    /// Behaviour used to generates the world
    /// </summary>
    public class WorldGenerator : MonoBehaviour, ICustomWorldData
    {
        private TilemapManager _tilemapManager;
        private WorldSettings _worldSettings;

        public int WorldSeed { get; private set; }
        public int TotalSteps { get; private set; }
        public int CurrentStep { get; private set; }
        public string CurrentStepMessage { get; private set; }

        [SerializeField] private GenerationGraph generationGraph;

        private void Awake()
        {
            _tilemapManager = FindObjectOfType<TilemapManager>();
        }
        
        private void Start()
        {
            // if not starting from this scene we don't generate the world here, something else should handle the world generation
            if (GlobalData.HasKey(GlobalDataKeys.GameState)) return;
            
            const int width = 256;
            const int height = 256;
            
            GlobalData.Set(GlobalDataKeys.CurrentWorldSettings, WorldSettings.Default);
            StartCoroutine(Generate(width, height, -width / 2, -height / 2));
        }

        public IEnumerator CreateWorld()
        {
            _worldSettings = GlobalData.Read(GlobalDataKeys.CurrentWorldSettings);
            WorldSeed = _worldSettings.Seed;
            var width = _worldSettings.Width;
            var height = _worldSettings.Height;
            yield return Generate(width, height, -width / 2, -height / 2);
        }

        public void GenerateArea(int width, int height, int xOffset, int yOffset)
        {
            StartCoroutine(Generate(width, height, xOffset, yOffset));
        }

        private IEnumerator Generate(int width, int height, int xOffset, int yOffset)
        {
            _tilemapManager.UpdateTiles = false;
            var root = generationGraph.nodes.Find(node => node is TerrainGeneratorRoot);

            if (root is not TerrainGeneratorRoot rootNode)
            {
                Debug.LogError($"No root node found in generation graph with name {name}");
                yield break;
            }

            rootNode.seed = WorldSeed;
            rootNode.xOffset = xOffset;
            rootNode.yOffset = yOffset;
            rootNode.width = width;
            rootNode.height = height;

            CurrentStep = 0;
            TotalSteps = generationGraph.nodes.Count(node => node is TerrainGenerator) + 1;

            var currentNode = rootNode.GetPort("nextStage");

            while (currentNode.IsConnected && currentNode.Connection.node is TerrainGenerator generatorNode)
            {
                if (generatorNode.IsDebugNode() && !generationGraph.EnableDebugNodes)
                {
                    currentNode = generatorNode.GetPort("nextStage");
                    continue;
                }

                CurrentStepMessage = generatorNode.StageMessage();
                generatorNode.Generate(WorldSeed, width, height, xOffset, yOffset, _tilemapManager);
                currentNode = generatorNode.GetPort("nextStage");
                CurrentStep++;

                yield return null;
            }

            _tilemapManager.UpdateTiles = true;

            // find the player object
            var player = GameObject.FindGameObjectsWithTag("Player");
            
            if (player.Length <= 0) yield break;
            
            var p = player[0].transform;
            var obstaclesLayer = _tilemapManager.ObstacleLayer;

            if (obstaclesLayer.GetTile(obstaclesLayer.WorldToCell(p.position)) == null)
            {
                CurrentStep++;
                yield break;
            }
            
            // try to find spawn point
            var eighthWidth = width / 16 + xOffset;
            var eighthHeight = height / 16 + yOffset;
            var spawnPoint = new Optional<Vector3Int>();
            
            while (!spawnPoint.Enabled)
            {
                // loop through a list of positions
                for (var x = -eighthWidth; x < eighthWidth; x++)
                {
                    for (var y = -eighthHeight; y < eighthHeight; y++)
                    {
                        var pos = new Vector3Int(x, y);
                        // if the position is not empty we go to the next one
                        if (obstaclesLayer.GetTile(pos) != null) continue;
                        
                        // if it is we have found a spawn point
                        spawnPoint = new Optional<Vector3Int>(pos);
                        break;
                    }
                }

                eighthWidth *= 2;
                eighthHeight *= 2;

                if (eighthWidth > (width + xOffset) / 2 || eighthHeight > (height + yOffset) / 2)
                {
                    break;
                }
            }

            if (!spawnPoint.Enabled)
            {
                Debug.Log("Can not find a valid spawn point");
                CurrentStep++;
                yield break;
            }

            p.position = obstaclesLayer.CellToWorld(spawnPoint.Value);
            CurrentStep++;
        }

        public SerializationPriority Priority => SerializationPriority.High;

        public async Task Save()
        {
            var worldSettingsSaveLocation = FileLocation.CurrentWorldSave(FileNameConstants.WorldSettingsFile);
            var currentWorldSettings = GlobalData.Read(GlobalDataKeys.CurrentWorldSettings);
            await SaveUtilities.Save(worldSettingsSaveLocation, currentWorldSettings);
        }

        public async Task Read(FileLocation worldFolder)
        {
            var worldSettings = await SaveUtilities.Load<WorldSettings>(worldFolder.FileAtLocation(FileNameConstants.WorldSettingsFile), null);
            GlobalData.Set(GlobalDataKeys.CurrentWorldSettings, worldSettings);
        }
    }
}