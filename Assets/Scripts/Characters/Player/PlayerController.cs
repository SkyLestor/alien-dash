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

        private PlayerRegistry _playerRegistry;

        public MovementController MovementController { get; private set; }
        public PlayerStats Stats { get; private set; }

        public IAnimationsController AnimationsController { get; private set; }

        [Inject]
        public void Construct(PlayerRegistry playerRegistry)
        {
            _playerRegistry = playerRegistry;
        }

        private void Awake()
        {
            Stats = new PlayerStats(100, 10, 3, 1.2f);
            AnimationsController = new PlayerAnimationsController(_animator);
            MovementController = GetComponent<MovementController>();
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

            if (MovementController.CurrentState == MovementController.DashingState)
            {
                enemy.TakeDamage(Stats.Damage);
            }
            else
            {
                TakeDamage(enemy.Damage);
            }
        }


        public int CurrentHeath => Stats.CurrentHealth;

        public int MaxHealth => Stats.MaxHealth;

        public void TakeDamage(int damage)
        {
            Stats.CurrentHealth = Mathf.Clamp(CurrentHeath - damage, 0, MaxHealth);
            AnimationsController.PlayDamagedAnimation();
            EventBus.Raise(new PlayerDamagedEvent { Player = this });
            if (Stats.CurrentHealth == 0)
            {
                Destroy(gameObject);
            }
        }


        public class PlayerStats
        {
            public int CurrentHealth;
            public int Damage;
            public int DashCharges;
            public float DashesCooldown;
            public int MaxHealth;

            public PlayerStats(int initialHealth, int initialDamage, int dashCharges, float dashesCooldown)
            {
                MaxHealth = initialHealth;
                CurrentHealth = initialHealth;
                Damage = initialDamage;
                DashCharges = dashCharges;
                DashesCooldown = dashesCooldown;
            }
        }
    }
}