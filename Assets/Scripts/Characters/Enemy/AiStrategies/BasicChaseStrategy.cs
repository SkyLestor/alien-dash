using System.Collections;
using Scripts.Characters.Player;
using UnityEngine;

namespace Scripts.Characters.Enemy.AiStrategies
{
    public class BasicChaseStrategy : IAiStrategy
    {
        private readonly PlayerRegistry _playerRegistry;

        public BasicChaseStrategy(PlayerRegistry playerRegistry)
        {
            _playerRegistry = playerRegistry;
        }

        public IEnumerator InitializeMovementStrategy(EnemyController controller)
        {
            while (controller)
            {
                var player = _playerRegistry.GetClosestPlayer(controller.transform.position);
                if (player)
                {
                    var directionToPlayerNormalized =
                        (player.transform.position - controller.transform.position).normalized;
                    controller.transform.position +=
                        directionToPlayerNormalized * (controller.Config.MaxSpeed * Time.deltaTime);
                }

                yield return null;
            }
        }
    }
}