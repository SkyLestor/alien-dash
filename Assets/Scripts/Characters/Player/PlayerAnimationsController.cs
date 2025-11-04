using DG.Tweening;
using UnityEngine;

namespace Scripts.Characters.Player
{
    public class PlayerAnimationsController : IAnimationsController
    {
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int IsDashing = Animator.StringToHash("IsDashing");
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");

        private readonly Animator _animator;
        private readonly MaterialPropertyBlock _block = new();
        private readonly SpriteRenderer _spriteRenderer;

        private Tween _flashTween;

        public PlayerAnimationsController(Animator animator)
        {
            _animator = animator;
            _spriteRenderer = animator.GetComponent<SpriteRenderer>();
        }

        public void PlayIdleAnimation()
        {
            _animator.SetBool(IsRunning, false);
        }

        public void PlayRunAnimation()
        {
            _animator.SetBool(IsRunning, true);
        }

        public void StartDashAnimation()
        {
            _animator.SetBool(IsDashing, true);
        }

        public void FinishDashAnimation()
        {
            _animator.SetBool(IsDashing, false);
        }

        public void PlayDamagedAnimation()
        {
            FlashEffect(0.3f);
        }

        private void FlashEffect(float totalDuration)
        {
            _flashTween?.Kill();

            _flashTween = DOVirtual.Float(0f, 1f, totalDuration / 2f, SetFlashAmount)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .OnKill(() => SetFlashAmount(0f));
        }

        private void SetFlashAmount(float value)
        {
            _spriteRenderer.GetPropertyBlock(_block);
            _block.SetFloat(FlashAmount, value);
            _spriteRenderer.SetPropertyBlock(_block);
        }
    }
}