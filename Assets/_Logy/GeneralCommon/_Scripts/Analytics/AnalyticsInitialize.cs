using UnityEngine;

namespace Logy.Analytics
{
    public class AnalyticsInitialize : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}