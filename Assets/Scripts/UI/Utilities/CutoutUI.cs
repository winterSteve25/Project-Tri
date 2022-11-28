using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI.Utilities
{
    public class CutoutUI : Image
    {
        public override Material materialForRendering
        {
            get
            {
                var mat = new Material(base.materialForRendering);
                mat.SetInt("_StencilComp", (int) CompareFunction.NotEqual);
                return mat;
            }
        }
    }
}