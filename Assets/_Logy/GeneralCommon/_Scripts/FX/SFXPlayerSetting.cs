using UnityEngine;

namespace Logy.UnityCommonV01
{
    [CreateAssetMenu(fileName = nameof(SFXPlayerSetting), menuName = "ScriptableObject/" + nameof(SFXPlayerSetting))]
    public class SFXPlayerSetting : ScriptableObject
    {
        [field: SerializeField]
        public AudioClipSetting[] audioClipSettings { get; private set; }
    }
}