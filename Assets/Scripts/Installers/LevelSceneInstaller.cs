using Scripts.Characters.Enemy;
using Scripts.Characters.Enemy.AiStrategies;
using Scripts.Characters.Enemy.EnemyDrops;
using Scripts.Characters.Player;
using Scripts.RoundManagement;
using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private EnemyDrop _enemyDropPrefab;
        [SerializeField] private EnemiesContainerSo _enemiesContainerSo;

        public override void InstallBindings()
        {
            Container.Bind<EnemiesContainerSo>().FromInstance(_enemiesContainerSo).AsSingle().NonLazy();
            Container.Bind<IRoundManager>().To<RoundManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            Container.Bind<PlayerRegistry>().AsSingle().NonLazy();
            Container.Bind<PlayerController>().FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();

            Container.Bind<IAiStrategy>().WithId(EnemyType.Minion).To<BasicChaseStrategy>().AsSingle().NonLazy();


            Container.Bind<AiStrategyProvider>().AsSingle().NonLazy();


            foreach (var (type, prefabs) in _enemiesContainerSo.EnemyPrefabs)
            {
                foreach (var prefab in prefabs)
                {
                    if (!prefab)
                    {
                        continue;
                    }

                    Container.BindMemoryPool<EnemyController, EnemyPool>()
                        .WithId(type)
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(prefab)
                        .UnderTransformGroup(type.ToString()).NonLazy();
                }
            }

            Container.BindMemoryPool<EnemyDrop, EnemyDropsPool>()
                .WithInitialSize(50)
                .FromComponentInNewPrefab(_enemyDropPrefab)
                .UnderTransformGroup("EnemyDrops").NonLazy();
            Container.BindInterfacesTo<EnemyDropsProvider>().AsSingle().NonLazy();
        }
    }
}