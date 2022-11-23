using System.Linq;
using Items;
using Tiles.Container;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using World;

namespace Tiles.Excavator
{
    /// <summary>
    /// The behaviour of an excavator tile
    /// </summary>
    public class ExcavatorBehaviour : MachineTile
    {
        [SerializeField] private MiningRecipes recipes;
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem dustParticles;
        
        private TileBase _ore;
        private MiningRecipe _recipe;
        private bool _invalidRecipe;
        private float _progress;
        private bool _startedParticles;
        
        private ContainerBehaviour _container;
        private static readonly int Progress = Animator.StringToHash("Progress");

        protected override void Start()
        {
            base.Start();
            _ore = TilemapManager.GetTile(Pos, TilemapLayer.Ground);
            _recipe = recipes.FindRecipe(_ore);
            _invalidRecipe = _recipe.output.IsEmpty;
            GetContainer();
        }

        private void Update()
        {
            animator.SetFloat(Progress, _progress);
            if (_invalidRecipe)
            {
                _startedParticles = false;
                dustParticles.Stop();
                return;
            }

            if (!_startedParticles)
            {
                dustParticles.Play();
                _startedParticles = true;
            }
            
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
                ItemSpawner.Current.SpawnApproximatelyAt(pos, itemStack);
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
            
            var container = machineTiles.FirstOrDefault(machine =>
                machine is ContainerBehaviour containerBehaviour &&
                containerBehaviour.Inventory.Count < containerBehaviour.Inventory.SlotsCount);

            if (container != null)
            {
                _container = (ContainerBehaviour) container;
            }
        }
    }
}