using System;
using UnityEngine;

namespace Clickbait.Utilities
{
    [Serializable]
    public class Observer<T>
    {
        [SerializeField] T _value;
        Action<T> _onValueChanged;

        public T Value
        {
            get => _value;
            set => Set(value);
        }

        public Observer(T val, Action<T> callback = null)
        {
            _value = val;
            if (callback != null)
                _onValueChanged += callback;
        }

        void Set(T val)
        {
            if (Equals(_value, val))
                return;

            _value = val;
            Invoke();
        }

        public void Invoke()
        {
            _onValueChanged?.Invoke(_value);
        }

        public void AddListener(Action<T> callback)
        {
            if (callback == null)
                return;

            _onValueChanged += callback;
        }

        public void RemoveListener(Action<T> callback)
        {
            if (callback == null)
                return;

            _onValueChanged -= callback;
        }
    }
}