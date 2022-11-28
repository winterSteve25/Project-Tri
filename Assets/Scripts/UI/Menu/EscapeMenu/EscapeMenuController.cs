using System.Linq;
using System.Threading.Tasks;
using KevinCastejon.MoreAttributes;
using SaveLoad;
using SaveLoad.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Utils.Data;

namespace UI.Menu.EscapeMenu
{
    public class EscapeMenuController : CurrentInstanced<EscapeMenuController>
    {
        [SerializeField, Required] 
        [FoldoutGroup("Required Fields")]
        private CanvasGroup escapeMenu;

        [SerializeField, Scene, Required] 
        [FoldoutGroup("Required Fields")]
        private string titleScene;
        
        [SerializeField] 
        [FoldoutGroup("Configuration")]
        private float fadeTime = 0.15f;

        public bool IsActive => escapeMenu.gameObject.activeSelf;

        private void Start()
        {
            escapeMenu.Disable();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }

        public void ToggleMenu()
        {
            escapeMenu.FadeToggle(fadeTime);
        }

        public void SaveAndExit()
        {
            ExitWorld();
        }
        
        private async Task ExitWorld()
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
    }
}