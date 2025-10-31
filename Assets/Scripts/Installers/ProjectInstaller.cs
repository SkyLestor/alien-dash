using Scripts.Input;
using Zenject;

namespace Scripts.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputManager>().To<InputManager>().AsSingle().NonLazy();
        }
    }
}