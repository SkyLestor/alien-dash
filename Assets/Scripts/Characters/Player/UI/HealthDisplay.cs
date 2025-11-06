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
            _healthBar.fillAmount = (float)_playerController.CurrentHeath / _playerController.MaxHealth;
            _healthText.text = $"{_playerController.CurrentHeath} / {_playerController.MaxHealth}";
        }
    }
}