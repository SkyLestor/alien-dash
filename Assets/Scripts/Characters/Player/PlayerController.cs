using Scripts.Characters.Enemy;
using Scripts.GameEventBus;
using UnityEngine;
using Zenject;

namespace Scripts.Characters.Player
{
    [RequireComponent(typeof(MovementController))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private Animator _animator;

        private MovementController _movementController;

        private PlayerRegistry _playerRegistry;
        private PlayerStats _playerStats;

        public IAnimationsController AnimationsController { get; private set; }

        [Inject]
        public void Construct(PlayerRegistry playerRegistry)
        {
            _playerRegistry = playerRegistry;
        }

        private void Awake()
        {
            _playerStats = new PlayerStats(100, 10);
            AnimationsController = new PlayerAnimationsController(_animator);
            _movementController = GetComponent<MovementController>();
        }

        private void OnEnable()
        {
            _playerRegistry.Register(this);
        }

        private void OnDisable()
        {
            _playerRegistry.Unregister(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<EnemyController>(out var enemy))
            {
                return;
            }

            if (_movementController.CurrentState == _movementController.DashingState)
            {
                enemy.TakeDamage(_playerStats.Damage);
            }
            else
            {
                TakeDamage(enemy.Damage);
            }
        }


        public int CurrentHeath => _playerStats.CurrentHealth;

        public int MaxHealth => _playerStats.MaxHealth;

        public void TakeDamage(int damage)
        {
            _playerStats.CurrentHealth = Mathf.Clamp(CurrentHeath - damage, 0, MaxHealth);
            AnimationsController.PlayDamagedAnimation();
            EventBus.Raise(new PlayerDamagedEvent { Player = this });
            if (_playerStats.CurrentHealth == 0)
            {
                Destroy(gameObject);
            }
        }


        public class PlayerStats
        {
            public int CurrentHealth;
            public int Damage;

            public int MaxHealth;

            public PlayerStats(int initialHealth, int initialDamage)
            {
                MaxHealth = initialHealth;
                CurrentHealth = initialHealth;
                Damage = initialDamage;
            }
        }
    }
}