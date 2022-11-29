using System;
using UnityEngine;

namespace World.Sequence.PosGenerators
{
    public class DimensionalPosFunction : FunctionNode
    {
        public override Type ProductType => typeof(Vector3[]);
        
        protected override object SupplyInternal()
        {
            if (Width == 0 || Height == 0)
            {
                Debug.LogWarning("Dimensions should not be 0!");
                return new Vector3Int[] {};
            }
            
            var pos = new Vector3Int[Width * Height];
            var n = 0;
            
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                { 
                    pos[n] = new Vector3Int(i + XOffset, j + YOffset);
                    n++;
                }
            }
                
            return pos;
        }
    }
}