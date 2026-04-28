using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Logy.UnityCommonV01
{
    public class NextInput : MonoBehaviour, IPointerDownHandler
    {
        private Action _pointerDownEvent;

        public void AddPointerDownListener(Action _listener) => _pointerDownEvent += _listener;

        public void RemovePointerDownListener(Action _listener) => _pointerDownEvent -= _listener;

        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDownEvent?.Invoke();
        }
    }
}