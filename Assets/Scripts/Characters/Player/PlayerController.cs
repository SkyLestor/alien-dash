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
        public PlayerData Data { get; private set; }

        public IAnimationsController AnimationsController { get; private set; }

        [Inject]
        public void Construct(PlayerRegistry playerRegistry, PlayerConfigSo config)
        {
            _playerRegistry = playerRegistry;
            Data = new PlayerData(config.PlayerData);
        }

        private void Awake()
        {
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
                enemy.TakeDamage(Data.Damage);
            }
            else
            {
                TakeDamage(enemy.Damage);
            }
        }


        public int CurrentHeath => Data.CurrentHealth;

        public int MaxHealth => Data.MaxHealth;

        public void TakeDamage(int damage)
        {
            Data.TakeDamage(damage);
            AnimationsController.PlayDamagedAnimation();
            EventBus.Raise(new PlayerDamagedEvent { Player = this });
            if (Data.CurrentHealth == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}