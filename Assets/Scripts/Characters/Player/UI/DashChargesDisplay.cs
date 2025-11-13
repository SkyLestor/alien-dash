using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Characters.Player.UI
{
    public class DashChargesDisplay : MonoBehaviour
    {
        [SerializeField] private Image _chargeImagePrefab;

        private PlayerController _playerController;

        private readonly List<Image> _charges = new();

        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
        }
        
        private void Update()
        {
            if (!_playerController || _playerController.MovementController.DashRecoveryProgress == 0)
            {
                return;
            }

            var maxCharges = _charges.Count;
            var fullCharges = _playerController.MovementController.CurrentDashCharges;

            var recoveryProgress =
                _playerController.MovementController.DashRecoveryProgress / _playerController.Stats.DashesCooldown;

            for (var i = 0; i < maxCharges; i++)
            {
                if (i < fullCharges)
                {
                    _charges[i].fillAmount = 1;
                }
                else if (i == fullCharges)
                {
                    _charges[i].fillAmount = recoveryProgress;
                }
                else
                {
                    _charges[i].fillAmount = 0;
                }
            }
        }

        private void OnEnable()
        {
            var maxCharges = _playerController.Stats.DashCharges;

            for (var i = 0; i < maxCharges; i++)
            {
                _charges.Add(Instantiate(_chargeImagePrefab, transform));
            }
        }

        private void OnDisable()
        {
            foreach (var image in _charges)
            {
                if (image)
                {
                    Destroy(image.gameObject);
                }
            }

            _charges.Clear();
        }
    }
}