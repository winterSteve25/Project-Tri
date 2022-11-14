using System.Linq;
using Items;
using Tiles.Container;
using UnityEngine;
using UnityEngine.Tilemaps;
using World;

namespace Tiles.Excavator
{
    /// <summary>
    /// The behaviour of an excavator tile
    /// </summary>
    public class ExcavatorBehaviour : MachineTile
    {
        [SerializeField] private ExcavatorRecipes recipes;

        private TileBase _ore;
        private ExcavatorRecipe _recipe;
        private bool _validRecipe;
        private float _progress;

        private ContainerBehaviour _container;

        protected override void Start()
        {
            base.Start();
            _ore = _tilemapManager.GetTile(Pos, TilemapLayer.Ground);
            _recipe = recipes.recipes.Find(r => r.ore == _ore);
            _validRecipe = _recipe.output.IsEmpty;
            GetContainer();
        }

        private void Update()
        {
            if (_validRecipe) return;
            _progress += Time.deltaTime;

            if (_progress >= _recipe.duration)
            {
                SpawnOutput();
                _progress = 0;
            }
        }

        public override void OnNeighborsChanged()
        {
            GetContainer();
        }

        private void SpawnOutput()
        {
            var pos = transform.position;
            var itemStack = _recipe.output;

            if (_container is null)
            {
                ItemSpawner.Instance.SpawnApproximatelyAt(pos, itemStack);
            }
            else
            {
                _container.Inventory.Add(pos, itemStack);
            }
        }

        private void GetContainer()
        {
            var machineTiles = GetNeighbors();
            if (machineTiles.Count <= 0) return;
            
            var container = machineTiles.First(machine =>
                machine is ContainerBehaviour containerBehaviour &&
                containerBehaviour.Inventory.Count < containerBehaviour.Inventory.SlotsCount);

            if (container != null)
            {
                _container = (ContainerBehaviour) container;
            }
        }
    }
}