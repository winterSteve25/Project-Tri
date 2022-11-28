using System.Collections;
using DG.Tweening;
using UnityEngine;
using Utils;
using World.Generation;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UI.Menu.CreateOrJoin.CreateWorld
{
    public class CreateWorldLoading : Loading
    {
        private IEnumerator Start()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;

            yield return StartCoroutine(LoadNextScene());
            var otherCamera = GetOtherCamera();
            
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
            yield return StartCoroutine(UnloadCurrentScene(currentScene, otherCamera));
        }
    }
}