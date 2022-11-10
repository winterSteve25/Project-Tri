using UnityEngine;
using UnityEngine.Tilemaps;

namespace Terrain
{
    /// <summary>
    /// Behaviour used to generates the world
    /// </summary>
    public class WorldGenerator : MonoBehaviour
    {
        private TilemapManager _tilemapManager;
        private int _seed;

        [SerializeField]
        private GenerationGraph generationGraph;

        private void Start()
        {
            _tilemapManager = FindObjectOfType<TilemapManager>();
            _seed = Random.Range(-5000, 5000);
            Generate(-256, -256, 512, 512);
        }

        public void Generate(int xOffset, int yOffset, int width, int height)
        {
            _tilemapManager.UpdateTiles = false;
            generationGraph.Generate(_seed, width, height, xOffset, yOffset, _tilemapManager);
            _tilemapManager.UpdateTiles = true;
        }
    }
}
