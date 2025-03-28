using UnityEngine;

namespace Clickbait.Utilities
{
    public class PersistentSingleton<T> : Singleton<T> where T : Component
    {
        /// <summary>
        /// Write your own class that derives PersistentSingleton<YourClass>
        /// Make sure you call base.Awake() if you write your own Awake method
        /// </summary>
        
        public bool AutoUnparentOnAwake = true;
        
        protected override void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            if (_instance == null) {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            } else {
                if (_instance != this) {
                    Destroy(gameObject);
                }
            }
            
            if (AutoUnparentOnAwake) {
                transform.SetParent(null);
            }
        }
    }
}