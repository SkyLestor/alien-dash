using UnityEngine;

namespace Scripts.Characters.Enemy
{
    [CreateAssetMenu(menuName = "Scriptable/EnemyConfig", fileName = "EnemyConfigSo")]
    public class EnemyConfigSo : ScriptableObject
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private int _damage;


        public int MaxHealth => _maxHealth;

        public float MaxSpeed => _maxSpeed;

        public int Damage => _damage;
    }
}