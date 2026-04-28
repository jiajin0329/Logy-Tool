using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Logy.UnityCommonV01
{
    public class LoopAnimation : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        private CancellationToken _ct;

        private void Start()
        {
            _ct = destroyCancellationToken;
            Loop().Forget();
        }

        private async UniTaskVoid Loop()
        {
            while (true)
            {
                await UniTask.Delay(100, cancellationToken: _ct);
                
                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                
                if (stateInfo.normalizedTime >= 1f)
                {
                    _animator.Play(stateInfo.shortNameHash, 0, 0f);
                }
            }
        }
    }
}