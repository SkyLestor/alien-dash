using Scripts.Characters.Enemy;
using UnityEngine;

namespace Scripts.Characters.Player
{
    [RequireComponent(typeof(MovementController))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private Animator _animator;
        private MovementController _movementController;
        private PlayerStats _playerStats;

        public IAnimationsController AnimationsController { get; private set; }

        private void Awake()
        {
            _playerStats = new PlayerStats(100, 10);
            AnimationsController = new PlayerAnimationsController(_animator);
            _movementController = GetComponent<MovementController>();
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