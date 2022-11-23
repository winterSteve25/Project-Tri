using System;
using SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu.CreateOrJoin.JoinWorld
{
    public class WorldButton : MonoBehaviour, IPointerClickHandler
    {
        [NonSerialized] public bool Selected;
        public event Action OnClick;
        
        public FileLocation FileLocation { get; private set; }
        
        public void Setup(string worldName)
        {
            FileLocation = FileLocation.WorldSavesFolder(worldName);
            GetComponentInChildren<TextMeshProUGUI>().text = worldName;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
            Selected = !Selected;
        }
    }
}