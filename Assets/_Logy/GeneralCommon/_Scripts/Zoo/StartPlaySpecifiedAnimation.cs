using UnityEngine;

namespace Logy.UnityCommonV01
{
    public class StartPlaySpecifiedAnimation : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private AnimationClip _animationClip;

        private void Awake()
        {
            if (_animationClip == null)
                return;

            _animator.Play(_animationClip.name);
        }
    }
}