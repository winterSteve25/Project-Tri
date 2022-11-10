using System;
using System.Linq;
using Terrain.Nodes.Utils;
using UnityEngine;
using XNode;

namespace Terrain.Nodes
{
    [CreateNodeMenu("")]
    public class TerrainGenerator : Node
    {
        [SerializeField, Input(ShowBackingValue.Never)] private byte lastStage;
        [SerializeField, Output] private byte nextStage;

        public virtual void Generate(int seed, int width, int height, int xOffset, int yOffset, TilemapManager tilemapManager)
        {
            throw new NotImplementedException();
        }

        public virtual string StageMessage()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsDebugNode()
        {
            return false;
        }

        public override object GetValue(NodePort port)
        {
            return port.fieldName switch
            {
                nameof(nextStage) => GetInputValue<int>(nameof(lastStage)) + 1,
                nameof(lastStage) => GetInputValue<int>(nameof(lastStage)),
                _ => base.GetValue(port)
            };
        }

        protected void AddNoiseInput()
        {
            if (DynamicInputs.All(port => port.fieldName != NoiseGenerator.FieldName))
            {
                AddDynamicInput(typeof(float[,]), fieldName: NoiseGenerator.FieldName);
            }
        }

        protected float[,] GetNoiseInput()
        {
            return GetInputValue<float[,]>(NoiseGenerator.FieldName);
        }
    }
}