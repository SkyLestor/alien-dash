using DG.Tweening;
using Scripts.GameEventBus;
using Scripts.RoundManagement;
using UnityEngine;
using Zenject;

namespace Scripts.Characters.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable, IDamaging, IPoolable<Vector3, Quaternion, IMemoryPool>
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


        private IMemoryPool _pool;


        public EnemyConfigSo Config => _config;

        [Inject]
        public void Construct(AiStrategyProvider provider)
        {
            _aiStrategy = provider.GetStrategy(_type);
        }

        private void Awake()
        {
            _block = new MaterialPropertyBlock();
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

        public void OnSpawned(Vector3 position, Quaternion rotation, IMemoryPool pool)
        {
            EventBus.Subscribe<GamePhaseChangedEvent>(OnGamePhaseChanged);


            _pool = pool;

            transform.SetPositionAndRotation(position, rotation);
            CurrentHeath = MaxHealth;
            _aiCoroutine = StartCoroutine(_aiStrategy.InitializeMovementStrategy(this));

            SetFlashAmount(0f);
            SetAlphaAmount();
        }

        public void OnDespawned()
        {
            EventBus.Unsubscribe<GamePhaseChangedEvent>(OnGamePhaseChanged);
            StopAiHandler();
            _animationSequence?.Kill();
            _pool = null;
        }

        private void OnGamePhaseChanged(GamePhaseChangedEvent eventData)
        {
            if (eventData.CurrentPhase is GamePhase.Finish or GamePhase.Upgrade)
            {
                _pool.Despawn(this);
            }
        }

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
                .OnComplete(() => _pool.Despawn(this));
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

    public class EnemyPool : MonoMemoryPool<Vector3, Quaternion, IMemoryPool, EnemyController>
    {
        protected override void Reinitialize(Vector3 p1, Quaternion p2, IMemoryPool p3, EnemyController item)
        {
            item.OnSpawned(p1, p2, p3);
            base.Reinitialize(p1, p2, p3, item);
        }

        protected override void OnDespawned(EnemyController item)
        {
            item.OnDespawned();
            base.OnDespawned(item);
        }
    }
}