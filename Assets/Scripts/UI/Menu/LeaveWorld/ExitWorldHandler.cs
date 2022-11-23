using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using KevinCastejon.MoreAttributes;
using SaveLoad;
using SaveLoad.Interfaces;
using UnityEngine;
using Utils;
using Utils.Data;

namespace UI.Menu.LeaveWorld
{
    public class ExitWorldHandler : MonoBehaviour
    {
        [SerializeField] private CanvasGroup escapeMenu;
        [Scene, SerializeField] private string titleScene;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (escapeMenu.gameObject.activeSelf)
                {
                    escapeMenu.DOFade(0, 1f)
                        .OnComplete(() => escapeMenu.gameObject.SetActive(false));
                }
                else
                {
                    escapeMenu.gameObject.SetActive(true);
                    escapeMenu.DOFade(1, 1f);
                }
            }
        }

        public void Save()
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