using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _playerPrefab;

        public override void InstallBindings()
        {
        }
    }
}