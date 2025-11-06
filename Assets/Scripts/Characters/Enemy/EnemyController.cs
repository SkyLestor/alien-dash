using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Scripts.Characters.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable, IDamaging
    {
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
        private static readonly int AlphaAmount = Shader.PropertyToID("_AlphaAmount");


        [SerializeField] private EnemyType _type;

        [Header("References")]
        [SerializeField] private EnemyConfigSo _config;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Animation settings")]
        [SerializeField] private float _xShakeStrength;

        [SerializeField] private float _yShakeStrength;
        [SerializeField] private float _damageAnimationDuration = 0.4f;


        private Coroutine _aiCoroutine;

        private IAiStrategy _aiStrategy;
        private Sequence _animationSequence;

        private MaterialPropertyBlock _block;


        public EnemyConfigSo Config => _config;

        [Inject]
        public void Construct(AiStrategyProvider provider)
        {
            _aiStrategy = provider.GetStrategy(_type);
        }

        private void Awake()
        {
            CurrentHeath = MaxHealth;
            _block = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            _aiCoroutine = StartCoroutine(_aiStrategy.InitializeMovementStrategy(this));
        }

        private void OnDisable()
        {
            StopAiHandler();
        }

        public int CurrentHeath { get; private set; }

        public int MaxHealth => Config.MaxHealth;

        public void TakeDamage(int damage)
        {
            CurrentHeath = Mathf.Clamp(CurrentHeath - damage, 0, MaxHealth);
            if (CurrentHeath == 0)
            {
                OnDeath();
            }
            else
            {
                DamagedAnimation();
            }
        }

        public int Damage => Config.Damage;

        private void StopAiHandler()
        {
            if (_aiCoroutine != null)
            {
                StopCoroutine(_aiCoroutine);
            }
        }

        private void DamagedAnimation()
        {
            _animationSequence?.Kill();

            var originalPos = transform.localPosition;

            _animationSequence = DOTween.Sequence()
                .Join(DOVirtual.Float(0f, 1f, _damageAnimationDuration / 2f, SetFlashAmount)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine))
                .Join(transform.DOShakePosition(
                    _damageAnimationDuration,
                    new Vector3(_xShakeStrength, _yShakeStrength, 0),
                    10,
                    45))
                .OnComplete(() =>
                {
                    transform.localPosition = originalPos;
                    SetFlashAmount(0f);
                });
        }

        private void OnDeath()
        {
            StopAiHandler();

            _animationSequence?.Kill();

            _animationSequence = DOTween.Sequence()
                .Join(DOVirtual.Float(1f, 0f, _damageAnimationDuration, SetFlashAmount)
                    .SetEase(Ease.InOutSine))
                .Join(DOVirtual.Float(1f, 0f, _damageAnimationDuration, SetAlphaAmount)
                    .SetEase(Ease.InOutSine))
                .OnComplete(() => Destroy(gameObject));
        }

        private void SetFlashAmount(float flashValue)
        {
            _spriteRenderer.GetPropertyBlock(_block);
            _block.SetFloat(FlashAmount, flashValue);
            _spriteRenderer.SetPropertyBlock(_block);
        }

        private void SetAlphaAmount(float alphaValue = 1)
        {
            _spriteRenderer.GetPropertyBlock(_block);
            _block.SetFloat(AlphaAmount, alphaValue);
            _spriteRenderer.SetPropertyBlock(_block);
        }
    }
}