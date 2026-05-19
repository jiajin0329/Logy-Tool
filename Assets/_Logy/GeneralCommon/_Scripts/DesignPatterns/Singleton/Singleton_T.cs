using System;

namespace Logy.UnityCommonV01
{
    public abstract class Singleton<T> where T : new()
    {
        protected Singleton() { }

        private static readonly Lazy<T> _instance = new Lazy<T>(() =>
            (T)Activator.CreateInstance(typeof(T), true));

        public static T Instance => _instance.Value;
    }
}
