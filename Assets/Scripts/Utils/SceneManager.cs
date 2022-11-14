
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneManager : Singleton<SceneManager>
    {
        public void LoadScene(string scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene, mode);
        }

        public void LoadScene(int scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene, mode);
        }
    }
}