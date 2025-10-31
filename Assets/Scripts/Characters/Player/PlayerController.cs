using UnityEngine;

namespace Scripts.Characters.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _maxHealth = 100;

        private void Awake()
        {
            CurrentHeath = _maxHealth;
        }

        public int CurrentHeath { get; private set; }

        public int MaxHealth => _maxHealth;

        public void TakeDamage(int damage)
        {
            CurrentHeath = Mathf.Clamp(CurrentHeath - damage, 0, MaxHealth);
        }
    }
}