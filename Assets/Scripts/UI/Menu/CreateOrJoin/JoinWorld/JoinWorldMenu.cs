using System.Collections.Generic;
using System.Linq;
using SaveLoad;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Data;

namespace UI.Menu.CreateOrJoin.JoinWorld
{
    public class JoinWorldMenu : Menu
    {
        [SerializeField] private Transform content;
        [SerializeField, AssetsOnly] private WorldButton worldItemPrefab;
        
        private List<WorldButton> _worldButtons;
        private FileLocation _location;
        
        private void OnEnable()
        {
            if (_location == null)
            {
                _location = FileLocation.WorldSavesFolder();
                _location.CreateDirectoryIfAbsent(null);
            }
            
            _worldButtons ??= new List<WorldButton>();
            PopulateWorldSelection();
        }

        public void JoinWorld()
        {
            var selectedWorld = _worldButtons.First(btn => btn.Selected);
            if (selectedWorld == null) return;
            
            GlobalData.Set(GlobalDataKeys.GameState, GameState.JoiningWorld);
            GlobalData.Set(GlobalDataKeys.JoinWorldLocation, selectedWorld.FileLocation);

            StartCoroutine(NextScene());
        }

        public void PopulateWorldSelection()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var worldSave in _location.Directories(null))
            {
                var worldItem = Instantiate(worldItemPrefab, content);
                worldItem.JoinWorldMenu = this;
                worldItem.Setup(worldSave[(worldSave.LastIndexOf('/') + 1)..]);
                worldItem.OnClick += ButtonClicked;
                _worldButtons.Add(worldItem);
            }
        }

        private void ButtonClicked()
        {
            foreach (var worldButton in _worldButtons)
            {
                worldButton.Selected = false;
            }
        }
    }
}
