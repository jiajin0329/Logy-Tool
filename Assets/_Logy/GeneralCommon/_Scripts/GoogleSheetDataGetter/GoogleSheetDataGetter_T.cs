using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Logy.UnityCommonV01
{
    public abstract class GoogleSheetDataGetter<T> : ScriptableObject
    {
        [SerializeField]
        private bool _loadOnInitialize;

        [SerializeField]
        private string _googleSheetURL;

        [SerializeField]
        private byte _pageNumber = 1;

        [SerializeField]
        private T[] _dataArray;

        public void Initialize()
        {
            if (_loadOnInitialize)
                _GetGoogleSheetDatas();
        }

        protected void _GetGoogleSheetDatas()
        {
            if (string.IsNullOrEmpty(_googleSheetURL))
                throw new ArgumentException("Google Sheet URL is null or empty.");

            using var _request = UnityWebRequest.Get(ConvertURL());
            var _asyncOperation = _request.SendWebRequest();

            while (!_asyncOperation.isDone)
            {
                Thread.Sleep(100);
            }

            if (_request.result != UnityWebRequest.Result.Success)
            {
                Logy.UnityCommonV01.Debug.LogError($"Failed to load Google Sheet: {_request.error}");
                return;
            }

            var _jsonText = _request.downloadHandler.text;
            var _json = "{\"Items\":" + _jsonText + "}";
            _dataArray = JsonUtility.FromJson<Wrapper<T>>(_json).Items;
        }

        private string ConvertURL()
        {
            // Extract spreadsheet ID between "/d/" and the next "/"
            const string _prefix = "/d/";
            var _startIndex = _googleSheetURL.IndexOf(_prefix) + _prefix.Length;
            var _endIndex = _googleSheetURL.IndexOf('/', _startIndex);
            var _spreadsheetId = _googleSheetURL.Substring(_startIndex, _endIndex - _startIndex);
            var _convertURL = $"https://opensheet.elk.sh/{_spreadsheetId}/{_pageNumber}";

            Logy.UnityCommonV01.Debug.Log($"{nameof(ConvertURL)} : {_convertURL}");

            return _convertURL;
        }

        [Serializable]
        private class Wrapper<T> { public T[] Items; }
    }
}