using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using World.Tiles;

namespace UI.BreakProgress
{
    public class BreakProgressManager : CurrentInstanced<BreakProgressManager>
    {
        [SerializeField] private Sprite[] breakStages;
        [SerializeField] private BreakProgressVisual progressPrefab;

        private Dictionary<TileInstance, BreakProgressVisual> _miningTiles;

        private void Start()
        {
            _miningTiles = new Dictionary<TileInstance, BreakProgressVisual>();
        }
        
        private void Update()
        {
            foreach (var (tile, visual) in _miningTiles)
            {
                var percentageDone = tile.BreakProgress / tile.Tile.hardness;
                var spriteUsed = Mathf.FloorToInt(breakStages.Length * percentageDone);
                visual.tileMask.sprite = tile.Tile.Sprite;
                
                if (spriteUsed > 0 && spriteUsed < breakStages.Length)
                {
                    visual.progress.sprite = breakStages[spriteUsed];
                }
            }
        }

        public bool MineTile(TileInstance tileInstance, Vector3Int pos, Tilemap tilemap, float miningSpeedModifier = 1)
        {
            if (tileInstance == null) return false;
            if (tileInstance.Tile.hardness < 0) return false;

            if (!_miningTiles.ContainsKey(tileInstance))
            {
                var instantiated = Instantiate(progressPrefab, transform);
                instantiated.Pos = pos;
                instantiated.Tilemap = tilemap;
                instantiated.UpdatePosition();
                PlayerCameraControl.ScaledObjects.Add(instantiated.transform);
                _miningTiles.Add(tileInstance, instantiated);
            }

            tileInstance.BreakProgress += Time.deltaTime * miningSpeedModifier;
            if (!(tileInstance.BreakProgress >= tileInstance.Tile.hardness)) return false;
            
            CancelMining(tileInstance);
            return true;
        }

        public void CancelMining(TileInstance tileInstance)
        {
            if (tileInstance is null) return;
            if (!_miningTiles.ContainsKey(tileInstance)) return;
            var breakProgressVisual = _miningTiles[tileInstance];
            PlayerCameraControl.ScaledObjects.Remove(breakProgressVisual.transform);
            Destroy(breakProgressVisual.gameObject);
            _miningTiles.Remove(tileInstance);
            tileInstance.BreakProgress = 0;
        }
    }
}