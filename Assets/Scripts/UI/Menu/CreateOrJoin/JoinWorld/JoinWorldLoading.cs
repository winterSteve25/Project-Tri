using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using SaveLoad;
using SaveLoad.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Data;

namespace UI.Menu.CreateOrJoin.JoinWorld
{
    public class JoinWorldLoading : Loading
    {
        private IEnumerator Start()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;

            yield return StartCoroutine(LoadNextScene());
            var otherCamera = GetOtherCamera();
            
            var joiningWorld = GlobalData.Read(GlobalDataKeys.JoinWorldLocation);
            var data = FindObjectsOfType<MonoBehaviour>().OfType<ICustomWorldData>().ToArray();

            progressBar.DOValue(.25f, 0.8f)
                .SetEase(Ease.Linear);
            message.text = "Loading the most important data";
            var task = data.Where(d => d.Priority == SerializationPriority.High).Select(dataReadr => dataReadr.Read(joiningWorld)).ToList();
            var loadTask = Task.WhenAll(task);
            yield return new WaitUntil(() => loadTask.IsCompleted);
            
            progressBar.DOValue(.5f, 0.8f)
                .SetEase(Ease.Linear);
            message.text = "Loading the very important data";
            task = data.Where(d => d.Priority == SerializationPriority.Medium).Select(dataReadr => dataReadr.Read(joiningWorld)).ToList();
            var loadTaskM = Task.WhenAll(task);
            yield return new WaitUntil(() => loadTaskM.IsCompleted);
            
            progressBar.DOValue(.75f, 0.8f)
                .SetEase(Ease.Linear);
            message.text = "Loading the normally important data";
            task = data.Where(d => d.Priority == SerializationPriority.Low).Select(dataReadr => dataReadr.Read(joiningWorld)).ToList();
            var loadTaskL = Task.WhenAll(task);
            yield return new WaitUntil(() => loadTaskL.IsCompleted);
            
            progressBar.DOValue(1f, 0.8f)
                .SetEase(Ease.Linear);
            message.text = "Finished!";

            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(UnloadCurrentScene(currentScene, otherCamera));
        }
    }
}