using System;
using UnityEngine;

namespace Scripts.Characters.Player
{
    [CreateAssetMenu(menuName = "Scriptable/PlayerConfig", fileName = "PlayerConfigSo")]
    public class PlayerConfigSo : ScriptableObject
    {
        [SerializeField] private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;
    }

    [Serializable]
    public class PlayerData
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _damage;
        [SerializeField] private int _dashCharges;
        [SerializeField] private float _dashesCooldown;
        [SerializeField] private int _levelUpCoefficient;

        private int _experienceGathered;

        public PlayerData()
        {
            Initialize();
        }

        public PlayerData(PlayerData source)
        {
            _maxHealth = source._maxHealth;
            _damage = source.Damage;
            _dashCharges = source.DashCharges;
            _dashesCooldown = source.DashesCooldown;
            _levelUpCoefficient = source._levelUpCoefficient;

            Initialize();
        }

        public int MaxHealth => _maxHealth;
        public int CurrentHealth { get; private set; }
        public int Level { get; private set; }
        public int LevelsPending { get; private set; }

        public int ExperienceToNextLevel => _levelUpCoefficient * Level;
        public int Damage => _damage;
        public int DashCharges => _dashCharges;
        public float DashesCooldown => _dashesCooldown;
        public void AddExperience(int experience)
        {
            _experienceGathered += experience;
            CheckAbilityToLevelUp();
        }

        private void Initialize()
        {
            CurrentHealth = _maxHealth;
            Level = 1;
            LevelsPending = 0;
            _experienceGathered = 0;
        }

        private void CheckAbilityToLevelUp()
        {
            if (_experienceGathered < ExperienceToNextLevel)
            {
                return;
            }

            LevelUp();
            CheckAbilityToLevelUp();
        }

        private void LevelUp()
        {
            _experienceGathered = Mathf.Clamp(_experienceGathered - ExperienceToNextLevel, 0, _experienceGathered);
            Level++;
        }

        public void ClearPendingLevels()
        {
            LevelsPending = 0;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);
        }
    }
}