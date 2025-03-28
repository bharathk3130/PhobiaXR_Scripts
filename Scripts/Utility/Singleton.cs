using UnityEngine;

namespace Clickbait.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// Write your own class that derives Singleton<YourClass>
        /// Make sure you call base.Awake() if you write your own Awake method
        /// </summary>
        
        protected static T _instance;

        public static bool HasInstance => _instance != null;
        public static T TryGetInstance() => HasInstance ? _instance : null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();
                    if (_instance == null)
                    {
                        var go = new GameObject(typeof(T).Name + " Auto-Generated");
                        _instance = go.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            _instance = this as T;
        }
    }
}