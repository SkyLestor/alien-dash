using System.Collections.Generic;
using Zenject;

namespace Scripts.Characters.Enemy
{
    public class AiStrategyProvider
    {
        private readonly Dictionary<EnemyType, IAiStrategy> _cache = new();
        private readonly DiContainer _container;

        public AiStrategyProvider(DiContainer container)
        {
            _container = container;
        }

        public IAiStrategy GetStrategy(EnemyType type)
        {
            if (!_cache.TryGetValue(type, out var strategy))
            {
                strategy = _container.ResolveId<IAiStrategy>(type);
                _cache[type] = strategy;
            }

            return strategy;
        }
    }
}