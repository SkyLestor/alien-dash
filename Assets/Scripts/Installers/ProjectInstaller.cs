using Scripts.Characters.Player;
using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private PlayerConfigSo _playerConfig;

        public override void InstallBindings()
        {
            Container.Bind<PlayerConfigSo>().FromInstance(_playerConfig).AsSingle().NonLazy();
        }
    }
}