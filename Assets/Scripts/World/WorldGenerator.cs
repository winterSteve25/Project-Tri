using System.Collections;
using System.Linq;
using UnityEngine;
using Utils.Data;
using World.Nodes;
using Debug = UnityEngine.Debug;

namespace World
{
    /// <summary>
    /// Behaviour used to generates the world
    /// </summary>
    public class WorldGenerator : MonoBehaviour
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
            // only happens when this is being ran as the starting scene in unity editor, so useful for debugging
            if (GlobalData.HasKey(GlobalDataKeys.WorldSettings)) return;
            const int width = 256;
            const int height = 256;
            StartCoroutine(Generate(width, height, -width / 2, -height / 2));
        }

        public IEnumerator CreateWorld()
        {
            _worldSettings = GlobalData.Read(GlobalDataKeys.WorldSettings);
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
            TotalSteps = generationGraph.nodes.Count(node => node is TerrainGenerator);

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
        }
    }
}