using System.Collections.Generic;
using UnityEngine;
using World.Tiles;

namespace TileBehaviours
{
    /// <summary>
    /// Base behaviour of a tile that has custom functionalities
    /// </summary>
    public class CustomTileBehaviour : MonoBehaviour
    {
        [SerializeField] protected bool instantiateVariables = true;

        protected TilemapManager TilemapManager;
        public Vector3Int Pos { get; protected set; }
        public Vector2Int Pos2D => new(Pos.x, Pos.x);
        
        protected virtual void Start()
        {
            if (!instantiateVariables) return;
            TilemapManager = TilemapManager.Current;
            Pos = TilemapManager.ObstacleLayer.WorldToCell(transform.position);
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

        protected List<CustomTileBehaviour> GetNeighbors()
        {
            var machines = new List<CustomTileBehaviour>();
            
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var pos = new Vector3Int(i, j) + Pos;
                    var go = TilemapManager.GetGameObject(pos, TilemapLayer.Obstacles);
                    if (go is null) continue;
                    if (go.TryGetComponent<CustomTileBehaviour>(out var machine))
                    {
                        machines.Add(machine);
                    }
                }
            }

            return machines;
        } 
    }
}