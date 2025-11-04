using Scripts.Characters.Enemy;
using Scripts.Characters.Enemy.AiStrategies;
using Scripts.Characters.Player;
using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _playerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<PlayerRegistry>().AsSingle().NonLazy();
            Container.Bind<PlayerController>().FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();

            Container.Bind<IAiStrategy>().WithId(EnemyType.Minion).To<BasicChaseStrategy>().AsSingle().NonLazy();


            Container.Bind<AiStrategyProvider>().AsSingle().NonLazy();
        }
    }
}