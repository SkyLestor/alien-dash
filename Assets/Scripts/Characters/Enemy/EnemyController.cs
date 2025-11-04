using UnityEngine;

namespace Scripts.Characters.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable, IDamaging
    {
        [SerializeField] private EnemyConfigSo _config;

        private void Awake()
        {
            CurrentHeath = MaxHealth;
        }

        public int CurrentHeath { get; private set; }

        public int MaxHealth => _config.MaxHealth;

        public void TakeDamage(int damage)
        {
            CurrentHeath = Mathf.Clamp(CurrentHeath - damage, 0, MaxHealth);
            if (CurrentHeath == 0)
            {
                Destroy(gameObject);
            }
        }

        public int Damage => _config.Damage;
    }
}