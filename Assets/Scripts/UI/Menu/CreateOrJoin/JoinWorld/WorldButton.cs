using System;
using System.IO;
using SaveLoad;
using TMPro;
using UnityEngine;

namespace UI.Menu.CreateOrJoin.JoinWorld
{
    public class WorldButton : MonoBehaviour
    {
        [NonSerialized] public JoinWorldMenu JoinWorldMenu;
        [NonSerialized] public bool Selected;
        public event Action OnClick;

        [SerializeField] private TextMeshProUGUI text;
        
        public FileLocation FileLocation { get; private set; }
        
        public void Setup(string worldName)
        {
            FileLocation = FileLocation.WorldSavesFolder(worldName);
            text.text = worldName;
        }

        public void Click()
        {
            OnClick?.Invoke();
            Selected = !Selected;
        }

        public void DeleteWorld()
        {
            if (FileLocation == null) return;
            Directory.Delete(FileLocation.GetFullPath(null), true);
            JoinWorldMenu.PopulateWorldSelection();
        }
    }
}