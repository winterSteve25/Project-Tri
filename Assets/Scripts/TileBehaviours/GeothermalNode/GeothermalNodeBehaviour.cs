using Systems.Heat;
using UnityEngine;

namespace TileBehaviours.GeothermalNode
{
    public class GeothermalNodeBehaviour : CustomTileBehaviour
    {
        [SerializeField] private float strength = 5;
        [SerializeField] private float diminishRate = 1;

        protected override void Start()
        {
            base.Start();
            HeatManager.Current.AddHeatSource(Pos2D, strength, diminishRate);
        }
    }
}