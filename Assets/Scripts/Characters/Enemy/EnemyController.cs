using UnityEngine;
using Zenject;

namespace Scripts.Characters.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable, IDamaging
    {
        [SerializeField] private EnemyConfigSo _config;
        [SerializeField] private EnemyType _type;

        private Coroutine _aiCoroutine;

        private IAiStrategy _aiStrategy;

        public EnemyConfigSo Config => _config;

        [Inject]
        public void Construct(AiStrategyProvider provider)
        {
            _aiStrategy = provider.GetStrategy(_type);
        }

        private void Awake()
        {
            CurrentHeath = MaxHealth;
        }

        private void OnEnable()
        {
            _aiCoroutine = StartCoroutine(_aiStrategy.InitializeMovementStrategy(this));
        }

        private void OnDisable()
        {
            if (_aiCoroutine != null)
            {
                StopCoroutine(_aiCoroutine);
            }
        }

        public int CurrentHeath { get; private set; }

        public int MaxHealth => Config.MaxHealth;

        public void TakeDamage(int damage)
        {
            CurrentHeath = Mathf.Clamp(CurrentHeath - damage, 0, MaxHealth);
            if (CurrentHeath == 0)
            {
                Destroy(gameObject);
            }
        }

        public int Damage => Config.Damage;
    }
}