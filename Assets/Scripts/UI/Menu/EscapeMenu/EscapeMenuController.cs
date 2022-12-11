using System.Linq;
using KevinCastejon.MoreAttributes;
using SaveLoad;
using SaveLoad.Interfaces;
using Sirenix.OdinInspector;
using UI.Managers;
using UnityEngine;
using Utils;
using Utils.Data;

namespace UI.Menu.EscapeMenu
{
    public class EscapeMenuController : CurrentInstanced<EscapeMenuController>
    {
        [SerializeField, Scene] private string titleScene;
        
        [SerializeField, Required] private CanvasGroup escapeMenu;
        [SerializeField, Required] private EscapeMenuButtonAnimator buttonAnimator;
      
        public bool IsActive => escapeMenu.gameObject.activeSelf;

        private void Start()
        {
            escapeMenu.alpha = 0;
            escapeMenu.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleVisibility();
            }
        }

        private void ToggleVisibility()
        {
            if (!escapeMenu.gameObject.activeSelf)
            {
                UIManager.ToggleUI(escapeMenu);
                buttonAnimator.AnimateIn();
            }
            else
            {
                buttonAnimator.AnimateOut();
                UIManager.CloseUI();
            }
        }
        
        public async void ExitWorld()
        {
            await FindObjectsOfType<MonoBehaviour>()
                .OfType<ICustomWorldData>()
                .ToList()
                .Save();

            // clear global data
            GlobalData.Remove(GlobalDataKeys.GameState);
            GlobalData.Remove(GlobalDataKeys.CurrentWorldSettings);
            
            SceneUtilities.Instance.LoadScene(titleScene);
        }

        public void Settings()
        {
            
        }

        public void Resume()
        {
            ToggleVisibility();
        }
    }
}