using System;
using UnityEngine;

namespace Logy.UnityCommonV01
{
    [Serializable]
    public class AudioClipSetting
    {
        [field: SerializeField]
        public bool isEnable { get; private set; } = true;

        [field: SerializeField]
        public AudioName name { get; private set; }

        [field: SerializeField]
        public AudioClipData[] audioClipDatas { get; private set; }
    }

    [Serializable]
    public class AudioClipData
    {
        [field: SerializeField]
        public bool isEnable { get; private set; }

        [field: SerializeField]
        public bool preload { get; private set; }

        [field: SerializeField]
        public AudioClip audioClip { get; private set; }

        [field: SerializeField]
        public float volume { get; private set; }

        public AudioClipData(bool _isEnable)
        {
            isEnable = _isEnable;
            audioClip = null;
            volume = 1f;
        }
    }

    public enum AudioName : byte
    {
        /// <summary>
        /// 音效1
        /// </summary>
        audio1,

        /// <summary>
        /// 音效2
        /// </summary>
        audio2,

        /// <summary>
        /// 音效3
        /// </summary>
        audio3
    }
}