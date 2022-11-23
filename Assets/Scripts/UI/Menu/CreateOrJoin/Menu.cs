using System.Collections;
using KevinCastejon.MoreAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace UI.Menu.CreateOrJoin
{
    public class Menu : MonoBehaviour
    {
        [Scene, SerializeField] private string loadingScene;
        
        protected IEnumerator NextScene()
        {
            yield return SceneUtilities.Instance.StartAnimation();
            SceneManager.LoadScene(loadingScene);
        }
    }
}