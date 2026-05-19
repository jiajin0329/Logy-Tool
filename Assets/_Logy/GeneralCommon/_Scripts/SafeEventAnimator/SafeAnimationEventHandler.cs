using System;
using UnityEngine;

namespace Logy.UnityCommonV01
{
    [Serializable]
    public class SafeAnimationEventHandler
    {
        [field: SerializeField]
        public AnimationEnum animationEnum { get; private set; }

        [SerializeField]
        private AnimationClip _animationClip;

        [Serializable]
        public class Data
        {
            [field: SerializeField]
            public SafeAnimationEventEnum eventEnum { get; private set; }

            [field: SerializeField]
            public ushort targetFrame { get; private set; }

            [NonSerialized]
            public float targetNormalizedTime;

            [NonSerialized]
            public ushort count;
        }

        [field: SerializeField]
        public Data[] dataArray { get; private set; }

        private SafeAnimationListener _safeAnimationListener;

        public void Initialize(SafeAnimationListener _safeAnimationListener)
        {
            this._safeAnimationListener = _safeAnimationListener;

            foreach (var _data in dataArray)
            {
                _data.targetNormalizedTime = _data.targetFrame / (_animationClip.frameRate * _animationClip.length);
            }
        }

        public void Reset()
        {
            foreach (var _data in dataArray)
            {
                _data.count = 0;
            }
        }

        public void Tick(Animator _animator) => DidEventCall(_animator);

        private void DidEventCall(Animator _animator)
        {
            var _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (!_stateInfo.IsName(animationEnum.ToString()))
                return;

            var _currentNormalizedTime = _stateInfo.normalizedTime;

            foreach (var _data in dataArray)
            {
                ushort _crossedEventCount = CrossedEventCount(_currentNormalizedTime, _data.targetNormalizedTime);

                if (EventCallCondition(_data, _crossedEventCount))
                {
                    // 一定要先設定count，否則Reset註冊_safeAnimationListene count會無法歸零
                    _data.count = _crossedEventCount;

                    _safeAnimationListener.Execute(_data.eventEnum);
                }
            }
        }

        private ushort CrossedEventCount(float _currentNormalizedTime, float _targetNormalizedTime) => (ushort)(_currentNormalizedTime - _targetNormalizedTime + 1f);

        private bool EventCallCondition(Data _data, ushort _crossedEventCount)
        {
            if (_data.count > 0 && !_animationClip.isLooping)
                return false;

            return _data.count < _crossedEventCount;
        }
    }
}