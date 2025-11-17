using System;
using Scripts.Characters.Enemy.EnemyDrops;
using Scripts.GameEventBus;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.Characters.Enemy
{
    public class EnemyDropsProvider : IInitializable, IDisposable
    {
        private readonly EnemyDropsPool _enemyDropsPool;

        public EnemyDropsProvider(EnemyDropsPool enemyDropsPool)
        {
            _enemyDropsPool = enemyDropsPool;
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<EnemyDiedEvent>(ProvideEnemyDrops);
        }

        public void Initialize()
        {
            EventBus.Subscribe<EnemyDiedEvent>(ProvideEnemyDrops);
        }

        private void ProvideEnemyDrops(EnemyDiedEvent eventData)
        {
            var dropsAmount = Random.Range(1, 5);
            var enemyDeathPosition = eventData.Enemy.transform.position;
            for (var i = 0; i < dropsAmount; i++)
            {
                _enemyDropsPool.Spawn(enemyDeathPosition,
                    Mathf.CeilToInt(eventData.Enemy.Config.ExperienceDropped / (float)dropsAmount));
            }
        }
    }
}