using System;
using UnityEngine;

namespace Logy.UnityCommonV01
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception($"[{typeof(T).Name}] Instance is null. Make sure it exists in the scene.");

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                throw new Exception($"[{typeof(T).Name}] Duplicate instance detected on '{gameObject.name}'. Destroying it.");
                Destroy(gameObject);
                return;
            }

            _instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
