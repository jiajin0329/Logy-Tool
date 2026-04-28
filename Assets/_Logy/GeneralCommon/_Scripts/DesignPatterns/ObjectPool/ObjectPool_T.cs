using System;
using System.Collections.Generic;

namespace Logy.UnityCommonV01
{
    public class ObjectPool<T>
    {
        private Queue<T> _idleQueue = new();
        private HashSet<T> _usingHashSet = new();

        public delegate T EventReturnObject();
        public EventReturnObject createEvent;
        public Action<T> releaseEvent;
        public Action<T> destoryEvent;

        public int idleCount => _idleQueue.Count;
        public int usingCount => _usingHashSet.Count;

        public ObjectPool(EventReturnObject _createEvent, Action<T> _releaseEvent, Action<T> _destoryEvent = null, ushort _startAmount = 0)
        {
            createEvent = _createEvent;
            releaseEvent = _releaseEvent;
            destoryEvent = _destoryEvent;

            BuildObject(_startAmount);
        }

        private void BuildObject(ushort _startAmount)
        {
            while (_idleQueue.Count < _startAmount)
            {
                if (createEvent != null)
                {
                    T _object = createEvent();
                    _idleQueue.Enqueue(_object);
                }
                else
                {
                    Debug.Log($"{nameof(createEvent)} is null");
                    break;
                }
            }
        }

        /// <summary>
        /// Get object.
        /// </summary>
        public T Get()
        {
            if (createEvent == null)
            {
                Debug.Log($"{nameof(EventReturnObject)} is null");
                return default;
            }

            T _object;
            if (_idleQueue.Count > 0)
                _object = _idleQueue.Dequeue();
            else
                _object = createEvent();

            _usingHashSet.Add(_object);
            return _object;
        }

        /// <summary>
        /// Release object.
        /// </summary>
        public void Release(T _object)
        {
            if (_usingHashSet.Contains(_object))
                _usingHashSet.Remove(_object);

            _idleQueue.Enqueue(_object);

            releaseEvent.Invoke(_object);
        }

        public void ReleaseAll()
        {
            foreach (T _object in _usingHashSet)
            {
                releaseEvent.Invoke(_object);
                _idleQueue.Enqueue(_object);
            }

            _usingHashSet.Clear();
        }

        /// <summary>
        /// Destory object.
        /// </summary>
        public void Destory()
        {
            ClearUseingList();
            ClearIdleQueue();
        }

        private void ClearUseingList()
        {
            foreach (T _object in _usingHashSet)
            {
                destoryEvent?.Invoke(_object);
            }

            _usingHashSet.Clear();
        }

        private void ClearIdleQueue()
        {
            while (_idleQueue.Count > 0)
            {
                T _object = _idleQueue.Dequeue();
                destoryEvent?.Invoke(_object);
            }
        }
    }
}