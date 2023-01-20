using System.Collections.Generic;
using System.Linq;
using SaveLoad;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils.Data;

namespace UI.Menu.CreateOrJoin.JoinWorld
{
    public class JoinWorldMenu : Menu
    {
        [SerializeField] private Transform content;
        [SerializeField, AssetsOnly] private WorldButton worldItemPrefab;
        [SerializeField] private Button joinWorldButton;

        private List<WorldButton> _worldButtons;
        private FileLocation _location;

        private void Start()
        {
            joinWorldButton.interactable = false;
        }

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
                worldItem.OnClick += () =>
                {
                    foreach (var worldButton in _worldButtons)
                    {
                        worldButton.Selected = false;
                    }
                };
                worldItem.OnClickFinished += () =>
                {
                    joinWorldButton.interactable = _worldButtons.Any(btn => btn.Selected);
                };
                _worldButtons.Add(worldItem);
            }
        }
    }
}