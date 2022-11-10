using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T) this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }
    }
}