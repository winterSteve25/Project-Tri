using System.Collections.Generic;
using Items;
using Terrain;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Tiles
{
    /// <summary>
    /// Base behaviour of a tile that has custom functionalities
    /// </summary>
    public class MachineTile : MapTile
    {
        [SerializeField] protected List<ItemStack> drops;
        [SerializeField] protected bool instantiateVariables = true;

        protected TilemapManager _tilemapManager;
        protected Vector3Int Pos;

        protected virtual void Start()
        {
            if (!instantiateVariables) return;
            _tilemapManager = FindObjectOfType<TilemapManager>();
            Pos = _tilemapManager.ObstacleLayer.WorldToCell(transform.position);
        }

        public virtual void OnInteract()
        {
        }

        public virtual void OnBroken()
        {
        }

        public virtual void OnNeighborsChanged()
        {
        }

        public virtual List<ItemStack> GetDrops()
        {
            return drops;
        }

        protected List<MachineTile> GetNeighbors()
        {
            var machines = new List<MachineTile>();
            
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var pos = new Vector3Int(i, j) + Pos;
                    var go = _tilemapManager.GetGameObject(pos, TilemapLayer.Obstacles);
                    if (go is null) continue;
                    if (go.TryGetComponent<MachineTile>(out var machine))
                    {
                        machines.Add(machine);
                    }
                }
            }

            return machines;
        } 
    }
}