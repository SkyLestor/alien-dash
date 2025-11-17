using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Characters.Player.UI
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private Image _experienceBar;
        [SerializeField] private TextMeshProUGUI _levelText;

        private PlayerController _playerController;

        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
        }

        private void Update()
        {
            _experienceBar.fillAmount = (float)_playerController.Data.ExperienceGathered / _playerController.Data.ExperienceToNextLevel;
            _levelText.text = $"lv. {_playerController.Data.Level}";
        }
    }
}