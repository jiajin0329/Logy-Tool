using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Logy.UnityCommonV01
{
    public class SafeEventAnimator : MonoBehaviour
    {
        [field: SerializeField]
        public Animator animator { get; private set; }

        [SerializeField]
        private SafeAnimationEventHandler[] _animationEventHandlerArray;

        private SafeAnimationListener _safeAnimationListener = new();
        private Dictionary<string, SafeAnimationEventHandler> _animationEventHandlerDictionary = new();
        private StringBuilder _animationName = new();

        private void Awake()
        {
            // 建立動畫事件資料字典
            foreach (var _eventHandler in _animationEventHandlerArray)
            {
                _animationEventHandlerDictionary.Add(_eventHandler.animationEnum.ToString(), _eventHandler);
                _eventHandler.Initialize(_safeAnimationListener);
            }
        }

        public void Play(string _name, int _layer = 0, float _normalizedTime = 0f)
        {
            animator.Play(_name, _layer, _normalizedTime);
            _animationName.Clear();
            _animationName.Append(_name);

            if (_animationEventHandlerDictionary.ContainsKey(_name))
                _animationEventHandlerDictionary[_name].Reset();
        }

        private void Update() => TickEventHandler();

        private void TickEventHandler()
        {
            if (_animationName.Length == 0)
                return;

            var _key = _animationName.ToString();
            if (!_animationEventHandlerDictionary.ContainsKey(_key))
                return;

            _animationEventHandlerDictionary[_key].Tick(animator);
        }

        public void AddListener(SafeAnimationEventEnum _enum, Action _listener) => _safeAnimationListener.Add(_enum, _listener);

        public void RemoveListener(SafeAnimationEventEnum _enum, Action _listener) => _safeAnimationListener.Remove(_enum, _listener);
    }
}