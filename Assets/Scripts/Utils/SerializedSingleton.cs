using Sirenix.OdinInspector;

namespace Utils
{
    public class SerializedSingleton<T> : SerializedMonoBehaviour where T : SerializedSingleton<T>
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }
    }
}