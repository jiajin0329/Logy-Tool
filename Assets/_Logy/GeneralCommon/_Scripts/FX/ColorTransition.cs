using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Logy.UnityCommonV01
{
    public class ColorTransition : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        private GameObject _gameObject;
        private Tween _tween;
        private CancellationTokenSource _cancellationTokenSource = new();

        public void Awake()
        {
            _gameObject = _image.gameObject;
            SetImageAlpha(0f);
            _gameObject.SetActive(false);
        }

        public async UniTask FadeIn(float _fadeInTime)
        {
            if (_image.color.a > 0f)
                return;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();

            SetImageAlpha(0f);

            _gameObject.SetActive(true);

            _tween = _image.DOFade(1f, _fadeInTime).SetEase(Ease.Linear);
            await _tween.ToUniTask(cancellationToken: _cancellationTokenSource.Token);
        }

        private void SetImageAlpha(float _alpha)
        {
            var _color = _image.color;
            _color.a = _alpha;
            _image.color = _color;
        }

        public async UniTask FadeOut(float _fadeOutTime)
        {
            if (_gameObject == null)
                return;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();

            SetImageAlpha(1f);

            _gameObject.SetActive(true);

            _tween = _image.DOFade(0f, _fadeOutTime).SetEase(Ease.Linear);
            await _tween.ToUniTask(cancellationToken: _cancellationTokenSource.Token);

            _gameObject.SetActive(false);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
            _tween.Kill();
            if (_gameObject != null)
                _gameObject.SetActive(false);
        }
    }
}