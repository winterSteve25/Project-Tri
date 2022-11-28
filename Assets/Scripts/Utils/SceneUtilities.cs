using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneUtilities : Singleton<SceneUtilities>
    {
        private const float DelayBeforeTransition = 0.5f;
        
        [SerializeField] private Animator animator;
        [SerializeField] private float transitionTime;
        [SerializeField] private CanvasGroup fade;
        
        private static readonly int StartTrigger = Animator.StringToHash("Start");
        private static readonly int EndTrigger = Animator.StringToHash("End");

        private void Start()
        {
            fade.alpha = 0;
            fade.gameObject.SetActive(false);
        }

        public Coroutine LoadScene(string scene)
        {
            return StartCoroutine(Transition(scene));
        }

        public Coroutine LoadAdditive(string scene)
        {
            return StartCoroutine(Load(scene));
        }
        
        public Coroutine UnloadScene(int scene, bool startingAnimation = true)
        {
            return StartCoroutine(Unload(scene, startingAnimation));
        }

        private IEnumerator Transition(string scene)
        {
            animator.ResetTrigger(StartTrigger);
            animator.ResetTrigger(EndTrigger);
            
            yield return StartCoroutine(StartAnimation());
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
            yield return new WaitForSeconds(DelayBeforeTransition);
            yield return StartCoroutine(EndAnimation());
        }

        private IEnumerator Load(string scene)
        {
            animator.ResetTrigger(StartTrigger);
            animator.ResetTrigger(EndTrigger);
            
            yield return StartCoroutine(StartAnimation());
            var operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (!operation.isDone) yield return null;
            yield return new WaitForSeconds(DelayBeforeTransition);
            yield return StartCoroutine(EndAnimation());
        }

        private IEnumerator Unload(int scene, bool startingAnimation)
        {
            animator.ResetTrigger(StartTrigger);
            animator.ResetTrigger(EndTrigger);

            if (startingAnimation)
            {
                yield return StartCoroutine(StartAnimation());
            }
            var operation = SceneManager.UnloadSceneAsync(scene);
            while (!operation.isDone) yield return null;
            yield return new WaitForSeconds(DelayBeforeTransition);
            yield return StartCoroutine(EndAnimation());
        }

        public IEnumerator StartAnimation()
        {
            // animator.SetTrigger(Start);
            // yield return new WaitForSeconds(transitionTime);

            var duration = transitionTime * 0.25f;
            fade.FadeIn(duration);
            yield return new WaitForSeconds(duration);
        }

        public IEnumerator EndAnimation()
        {
            // animator.SetTrigger(End);
            
            var duration = transitionTime * 0.25f;
            fade.DOFade(0, duration);
            yield return new WaitForSeconds(duration);
            fade.gameObject.SetActive(false);
        }
    }
}