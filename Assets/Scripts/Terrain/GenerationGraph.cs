using System.Diagnostics;
using Terrain.Nodes;
using UnityEngine;
using XNode;
using Debug = UnityEngine.Debug;

namespace Terrain
{
    [CreateAssetMenu(fileName = "New Generation Graph", menuName = "Generation/New Terrain Generation Graph")]
    public class GenerationGraph : NodeGraph
    {
        [SerializeField] private bool enableDebugNodes;
        
        public void Generate(int seed, int width, int height, int xOffset, int yOffset, TilemapManager tilemapManager)
        {
            var root = nodes.Find(node => node is TerrainGeneratorRoot);

            if (root is not TerrainGeneratorRoot rootNode)
            {
                Debug.LogError($"No root node found in generation graph with name {name}");
                return;    
            }

            rootNode.seed = seed;
            rootNode.xOffset = xOffset;
            rootNode.yOffset = yOffset;
            rootNode.width = width;
            rootNode.height = height;

            var currentNode = rootNode.GetPort("nextStage");
            var stopwatch = new Stopwatch();
            
            while (currentNode.IsConnected && currentNode.Connection.node is TerrainGenerator generatorNode)
            {
                if (generatorNode.IsDebugNode() && !enableDebugNodes)
                {
                    currentNode = generatorNode.GetPort("nextStage");
                    continue;
                }
                
                stopwatch.Restart();
                Debug.Log($"{generatorNode.StageMessage()}");
                generatorNode.Generate(seed, width, height, xOffset, yOffset, tilemapManager);
                Debug.Log($"Finished! Took {stopwatch.ElapsedMilliseconds}ms");
                
                currentNode = generatorNode.GetPort("nextStage");
            }
        }
    }
}