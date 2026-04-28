using System;
using UnityEngine;

namespace Logy.UnityCommonV01
{
    [Serializable]
    public struct GameObjectArray
    {
        public GameObject[] array;

        public void SetActive(bool _set)
        {
            foreach (GameObject _gameObject in array)
            {
                _gameObject.SetActive(_set);
            }
        }
    }
}