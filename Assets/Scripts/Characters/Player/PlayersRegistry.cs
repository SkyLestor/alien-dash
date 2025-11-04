using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Characters.Player
{
    public class PlayerRegistry
    {
        private readonly List<PlayerController> _activePlayers = new();

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

            // Simple LINQ to find the closest.
            return _activePlayers
                .OrderBy(p => Vector3.Distance(p.transform.position, position))
                .FirstOrDefault();
        }
    }
}