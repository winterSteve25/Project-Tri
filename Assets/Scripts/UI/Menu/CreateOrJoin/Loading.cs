using System.Collections;
using System.Linq;
using KevinCastejon.MoreAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Menu.CreateOrJoin
{
    public class Loading : MonoBehaviour
    {
        [SerializeField] protected Slider progressBar;
        [SerializeField] protected TextMeshProUGUI message;
        [Scene, SerializeField] private string worldScene;
        [SerializeField] private Camera loadingScreenCamera;

        protected IEnumerator LoadNextScene()
        {
            yield return StartCoroutine(SceneUtilities.Instance.EndAnimation());
            var operation = SceneManager.LoadSceneAsync(worldScene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => operation.isDone);
        }

        protected Camera GetOtherCamera()
        {
            var otherCamera = FindObjectsOfType<Camera>().First(cam => cam != loadingScreenCamera);
            otherCamera.gameObject.SetActive(false);
            return otherCamera;
        }

        protected IEnumerator UnloadCurrentScene(int currentScene, Camera otherCamera)
        {
            yield return SceneUtilities.Instance.StartAnimation();
            
            loadingScreenCamera.gameObject.SetActive(false);
            otherCamera.gameObject.SetActive(true);

            yield return SceneUtilities.Instance.UnloadScene(currentScene, false);
        }
    }
}