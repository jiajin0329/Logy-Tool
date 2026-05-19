using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logy.UnityCommonV01
{
    [Serializable]
    public class SFXPlayer : SingletonMonoBehaviour<SFXPlayer>
    {
        [SerializeField]
        private SFXPlayerSetting _setting;

        [SerializeField]
        private AudioSource _playAudioSource;
        [SerializeField]
        private AudioSource _playOneShotAudioSource;

        /// <summary>
        /// 儲存所有Audio Clip的設定.
        /// </summary>
        private Dictionary<string, AudioClipData[]> _dictionaryListAudioClipData = new();

        /// <summary>
        /// 儲存所有Audio Clip的未播放Index.
        /// </summary>
        private Dictionary<string, List<int>> _dictionaryListUnplayedIndex = new();

        /// <summary>
        /// 儲存所有Audio Clip的最後播放Index.
        /// </summary>
        private Dictionary<string, int> _dictionaryListAudioClipDataLastIndex = new();

        /// <summary>
        /// 儲存所有Audio Clip的Cd Time.
        /// </summary>
        private Dictionary<string, float> _dictionaryCdTime = new();

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            BuildDictionaryListAudioClipData();
        }

        private void BuildDictionaryListAudioClipData()
        {
            string _key;
            foreach (var audioClipSetting in _setting.audioClipSettings)
            {
                if (!audioClipSetting.isEnable)
                    continue;

                _key = audioClipSetting.name.ToString();

                var _listAudioClipData = new List<AudioClipData>();
                foreach (var audioClipData in audioClipSetting.audioClipDatas)
                {
                    if (!audioClipData.isEnable)
                        continue;

                    _listAudioClipData.Add(audioClipData);

                    if (audioClipData.preload)
                        audioClipData.audioClip.LoadAudioData();
                }

                _dictionaryListAudioClipData.Add(_key, _listAudioClipData.ToArray());
            }
        }

        /// <summary>
        /// 不可以重疊的播放音效.
        /// </summary>
        public void Play(AudioName _name)
        {
            var _key = _name.ToString();

            if (!_dictionaryListAudioClipData.ContainsKey(_key))
                return;

            var _audioClipSetting = RandomAudioClipData(_name);

            _playAudioSource.clip = _audioClipSetting.audioClip;
            _playAudioSource.volume = _audioClipSetting.volume;
            _playAudioSource.Play();
        }

        // 更新 Cd Time和移除已結束的Cd Time，讓Audio Clip可以下次播放.
        private void Update()
        {
            if (_dictionaryCdTime.Count < 1)
                return;

            var _keys = _dictionaryCdTime.Keys.ToArray();

            foreach (var _key in _keys)
            {
                _dictionaryCdTime[_key] -= Time.deltaTime;
                if (_dictionaryCdTime[_key] <= 0)
                    _dictionaryCdTime.Remove(_key);
            }
        }

        public void Stop() => _playAudioSource.Stop();

        /// <summary>
        /// 可以重疊的播放音效.
        /// </summary>
        public void PlayOneShot(AudioName _name)
        {
            var _key = _name.ToString();

            if (!_dictionaryListAudioClipData.ContainsKey(_key))
                return;

            var _audioClipData = RandomAudioClipData(_name);

            _playOneShotAudioSource.PlayOneShot(_audioClipData.audioClip, _audioClipData.volume);
        }

        private AudioClipData RandomAudioClipData(AudioName _name)
        {
            var _key = _name.ToString();

            // 如果只有一個Audio Clip，則直接回傳.
            if (_dictionaryListAudioClipData[_key].Length < 2)
                return _dictionaryListAudioClipData[_key][0];
            // 如果沒有未播放Index，則建立一個新的未播放Index列表.
            else if (!_dictionaryListUnplayedIndex.ContainsKey(_key))
                _dictionaryListUnplayedIndex.Add(_key, CreateIndexList(_dictionaryListAudioClipData[_key].Length));
            // 如果未播放Index列表為空，則建立一個新的未播放Index列表.
            else if (_dictionaryListUnplayedIndex[_key].Count < 1)
                _dictionaryListUnplayedIndex[_key] = CreateIndexList(_dictionaryListAudioClipData[_key].Length);

            // 取得下一個要播放的Audio Clip.
            var _listUnplayedIndex = _dictionaryListUnplayedIndex[_key];
            var _listUnplayedIndex_Index = GetListUnplayedIndex_Index(_key);
            var _audioClipDataIndex = _listUnplayedIndex[_listUnplayedIndex_Index];
            var _audioClipData = _dictionaryListAudioClipData[_key][_audioClipDataIndex];

            // 儲存最後播放的Audio Clip.
            if (_dictionaryListAudioClipDataLastIndex.ContainsKey(_key))
                _dictionaryListAudioClipDataLastIndex[_key] = _audioClipDataIndex;
            else
                _dictionaryListAudioClipDataLastIndex.Add(_key, _audioClipDataIndex);

            // 移除已使用的Index.
            _dictionaryListUnplayedIndex[_key].RemoveAt(_listUnplayedIndex_Index);

            return _audioClipData;
        }

        private List<int> CreateIndexList(int _count)
        {
            var _list = new List<int>();
            for (int i = 0; i < _count; i++)
                _list.Add(i);
            return _list;
        }

        private int GetListUnplayedIndex_Index(string _key)
        {
            var _listUnplayedIndex = _dictionaryListUnplayedIndex[_key];

            int _listIndex = UnityEngine.Random.Range(0, _listUnplayedIndex.Count);
            int _index = _listUnplayedIndex[_listIndex];

            if (_dictionaryListAudioClipDataLastIndex.ContainsKey(_key))
            {
                // 如果取得的index和上次播放的index相同，則更換index
                if (_index == _dictionaryListAudioClipDataLastIndex[_key])
                {
                    _listIndex++;
                    _listIndex = _listIndex == _listUnplayedIndex.Count ? _listIndex - _listUnplayedIndex.Count : _listIndex;
                }
            }

            return _listIndex;
        }

        /// <summary>
        /// 可以重疊播放音效,但同個音效要等播放完成才可以再播放.
        /// </summary>
        public void PlayOneShotWhenAudioClipEnd(AudioName _name, Action<AudioClip> _playEvent = null)
        {
            var _key = _name.ToString();

            if (!_dictionaryListAudioClipData.ContainsKey(_key))
                return;

            if (_dictionaryCdTime.ContainsKey(_key))
                return;

            var _audioClipSetting = RandomAudioClipData(_name);

            _playOneShotAudioSource.PlayOneShot(_audioClipSetting.audioClip, _audioClipSetting.volume);
            _dictionaryCdTime.Add(_key, _audioClipSetting.audioClip.length);

            _playEvent?.Invoke(_audioClipSetting.audioClip);
        }
    }
}