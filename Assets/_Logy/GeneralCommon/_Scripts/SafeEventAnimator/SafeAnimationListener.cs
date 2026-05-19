using System;
using System.Collections.Generic;

namespace Logy.UnityCommonV01
{
    public class SafeAnimationListener
    {
        private Dictionary<string, Action> _listenerDictionary = new();

        public void Add(SafeAnimationEventEnum _enum, Action _listener)
        {
            var _key = _enum.ToString();

            if (!_listenerDictionary.ContainsKey(_key))
                _listenerDictionary.Add(_key, _listener);
            else
                _listenerDictionary[_key] += _listener;
        }

        public void Remove(SafeAnimationEventEnum _enum, Action _listener)
        {
            var _key = _enum.ToString();

            if (!_listenerDictionary.ContainsKey(_key))
                return;

            _listenerDictionary[_key] -= _listener;
        }

        public void Execute(SafeAnimationEventEnum _enum)
        {
            var _key = _enum.ToString();

            if (!_listenerDictionary.ContainsKey(_key))
                return;

            _listenerDictionary[_key]?.Invoke();
        }
    }
}