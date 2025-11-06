using System.Collections;
using Scripts.Characters.Player;
using UnityEngine;

namespace Scripts.Characters.Enemy.AiStrategies
{
    public class BasicChaseStrategy : IAiStrategy
    {
        private readonly Vector3 _flippedRotation = new(0, 180, 0);
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
                    if (directionToPlayerNormalized.x > 0)
                    {
                        controller.transform.eulerAngles = _flippedRotation;
                    }
                    else if (directionToPlayerNormalized.x < 0)
                    {
                        controller.transform.eulerAngles = Vector2.zero;
                    }
                }

                yield return null;
            }
        }
    }
}