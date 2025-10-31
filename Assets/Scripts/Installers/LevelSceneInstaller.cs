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
            Container.Bind<PlayerController>().FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();
        }
    }
}