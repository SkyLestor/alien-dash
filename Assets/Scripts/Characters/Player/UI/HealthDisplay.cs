using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Characters.Player.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _healthText;


        private PlayerController _playerController;

        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
        }

        private void Update()
        {
            _healthBar.fillAmount = 1 - (float)_playerController.MaxHealth / _playerController.CurrentHeath;
            _healthText.text = $"{_playerController.CurrentHeath} / {_playerController.MaxHealth}";
        }
    }
}