using UnityEngine;

namespace Utils
{
    public class CurrentInstanced<T> : MonoBehaviour where T : CurrentInstanced<T>
    {
        private static T _current;
        public static T Current => _current;

        protected virtual void Awake()
        {
            _current = (T) this;
        }
    }
}