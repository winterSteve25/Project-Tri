using System.Linq;
using UnityEngine;
using World.Generation.Nodes;
using XNode;

namespace World.Generation.XNode
{
    [CreateAssetMenu(fileName = "New Biome Graph", menuName = "New Biome Graph")]
    public class BiomeGraph : NodeGraph
    {
        public GenerationNode Build()
        {
            var firstNode = nodes.FirstOrDefault(node => node is TilePlacerXNode tilePlacerXNode && !tilePlacerXNode.GetInputPort("previousNode").IsConnected) as TilePlacerXNode;
            if (firstNode == null) return null;
            var nextPort = firstNode.GetOutputPort("nextNode");
            GenerationNode generationNode = firstNode.GetNode();
            
            while (nextPort.IsConnected && nextPort.Connection.node is TilePlacerXNode tilePlacerXNode)
            {
                generationNode = generationNode.Next(tilePlacerXNode.GetNode());
                nextPort = tilePlacerXNode.GetOutputPort("nextNode");
            }

            return generationNode.Build();
        }
    }
}