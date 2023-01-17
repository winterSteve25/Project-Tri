using Systems.Heat;
using UnityEngine;

namespace TileBehaviours.GeothermalNode
{
    public class GeothermalNodeBehaviour : CustomTileBehaviour
    {
        [SerializeField] private float strength = 5;
        [SerializeField] private float diminishRate = 1;

        public float Strength => strength;
        public float DiminishRate => diminishRate;

        protected override void Start()
        {
            base.Start();
            HeatManager.Current.AddHeatSource(Pos2D, strength, diminishRate);
        }
    }
}