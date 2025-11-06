using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.GameEventBus;
using UnityEngine;

namespace Scripts.Characters.Player
{
    public class PlayerRegistry : IDisposable
    {
        private readonly List<PlayerController> _activePlayers = new();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Register(PlayerController player)
        {
            if (!_activePlayers.Contains(player))
            {
                _activePlayers.Add(player);
            }
        }

        public void Unregister(PlayerController player)
        {
            _activePlayers.Remove(player);
        }

        public PlayerController GetClosestPlayer(Vector3 position)
        {
            if (_activePlayers.Count == 0)
            {
                return null;
            }

            return _activePlayers
                .OrderBy(p => Vector3.Distance(p.transform.position, position))
                .FirstOrDefault();
        }

        public void Initialize()
        {
            EventBus.Subscribe<PlayerDamagedEvent>(OnPlayerDamagedEvent);
        }

        private void OnPlayerDamagedEvent(PlayerDamagedEvent eventData)
        {
            if (eventData.Player.CurrentHeath <= 0 && _activePlayers.Contains(eventData.Player))
            {
                _activePlayers.Remove(eventData.Player);
                EventBus.Raise(
                    new ActivePlayersAmountChangedEvent { CurrentActivePlayersAmount = _activePlayers.Count });
            }
        }
    }
}