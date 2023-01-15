using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Systems.Heat
{
    public class HeatManager : CurrentInstanced<HeatManager>
    {
        private Dictionary<Vector2Int, float> _heatMap;

        private void Start()
        {
            _heatMap = new Dictionary<Vector2Int, float>();
        }

        public void AddHeatSource(Vector2Int position, float strength, float diminishRate)
        {
            var positions = CirclePositions(position, (int)(strength / diminishRate));
            for (var i = 0; i < positions.Count; i++)
            {
                foreach (var p in positions[i])
                {
                    var str = strength - diminishRate * i;

                    if (_heatMap.ContainsKey(p))
                    {
                        _heatMap[p] += str;
                    }
                    else
                    {
                        _heatMap.Add(p, str);
                    }
                }
            }    
        }

        private void OnDrawGizmos()
        {
            if (_heatMap == null) return;
            foreach (var (pos, strength) in _heatMap)
            {
                Gizmos.color = new Color(1f, 0, 0, 0.1f * strength);
                Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
            }
        }

        private static List<List<Vector2Int>> CirclePositions(Vector2Int center, int radius) {
            var rings = new List<List<Vector2Int>>();
            for (var r = 0; r < radius; r++) {
                var positions = new List<Vector2Int>();
                for (var x = center.x - radius; x <= center.x + radius; x++) {
                    for (var y = center.y - radius; y <= center.y + radius; y++) {
                        if (Mathf.Sqrt((x - center.x) * (x - center.x) + (y - center.y) * (y - center.y)) <= r + 1 && Mathf.Sqrt((x - center.x) * (x - center.x) + (y - center.y) * (y - center.y)) > r) {
                            positions.Add(new Vector2Int(x, y));
                        }
                    }
                }
                rings.Add(positions);
            }
            
            rings.Add(new List<Vector2Int> {center});
            
            return rings.OrderBy(r => r.Count).ToList();;
        }
    }
}