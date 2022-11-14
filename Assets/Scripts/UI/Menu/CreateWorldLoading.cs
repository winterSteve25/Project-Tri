using System.Collections;
using System.Linq;
using DG.Tweening;
using KevinCastejon.MoreAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using World;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UI.Menu
{
    public class CreateWorldLoading : MonoBehaviour
    {
        [Scene, SerializeField] private string worldScene;
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Camera loadingScreenCamera;
        
        private void Start()
        {
            StartCoroutine(CreateWorld());
        }

        private IEnumerator CreateWorld()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            var operation = SceneManager.LoadSceneAsync(worldScene, LoadSceneMode.Additive);

            yield return new WaitUntil(() => operation.isDone);
            var otherCamera = FindObjectsOfType<Camera>().First(cam => cam != loadingScreenCamera);
            otherCamera.gameObject.SetActive(false);
            
            var worldGenerator = FindObjectOfType<WorldGenerator>();
            StartCoroutine(worldGenerator.CreateWorld());

            while (worldGenerator.CurrentStep < worldGenerator.TotalSteps)
            {
                var progress = 1 - MathHelper.MapTo0_1(worldGenerator.TotalSteps, worldGenerator.CurrentStep);
                progressBar.DOValue(progress, 0.8f)
                    .SetEase(Ease.Linear);
                message.text = worldGenerator.CurrentStepMessage;
                yield return null;
            }

            progressBar.DOValue(1, 0.8f)
                .SetEase(Ease.Linear);
            message.text = worldGenerator.CurrentStepMessage;

            yield return new WaitForSeconds(1);
            
            loadingScreenCamera.gameObject.SetActive(false);
            otherCamera.gameObject.SetActive(true);
            
            yield return SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}