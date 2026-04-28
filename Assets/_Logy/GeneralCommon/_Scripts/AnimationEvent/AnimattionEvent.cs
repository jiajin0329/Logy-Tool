using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logy.UnityCommonV01
{
    public class AnimationEvent : MonoBehaviour
    {
        [field: SerializeField]
        public Animator animator { get; private set; }

        private Dictionary<string, Action> animationEventDictionary = new();

        private void Execute(Enum _enum)
        {
            var _name = _enum.ToString();

            if (!animationEventDictionary.ContainsKey(_name))
                return;

            animationEventDictionary[_name]?.Invoke();
        }

        public void AddAnimationEventListener(Enum _enum, Action _listener)
        {
            var _name = _enum.ToString();

            if (!animationEventDictionary.ContainsKey(_name))
                animationEventDictionary.Add(_name, _listener);
            else
                animationEventDictionary[_name] += _listener;
        }

        public void RemoveAnimationEventListener(Enum _enum, Action _listener)
        {
            var _name = _enum.ToString();

            if (!animationEventDictionary.ContainsKey(_name))
                return;

            animationEventDictionary[_name] -= _listener;
        }

        public enum Enum
        {
            start,
            end,
            event1,
            event2,
            event3,
            event4
        }
    }
}