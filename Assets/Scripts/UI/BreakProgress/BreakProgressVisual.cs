using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace UI.BreakProgress
{
    public class BreakProgressVisual : MonoBehaviour
    {
        public Slider progress;
        [NonSerialized] public Vector3Int Pos;
        [NonSerialized] public Tilemap Tilemap;
        
        private void Update()
        {
            UpdatePosition();
        }
        
        public void UpdatePosition() 
        {
            if (Tilemap is null) return;
            transform.position = Tilemap.CellToWorld(Pos) + 0.5f * Tilemap.cellSize;
        }
    }
}