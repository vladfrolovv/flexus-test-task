using Cameras;
using Canons;
using Inputs;
using Zenject;
namespace Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {

            Container.BindInterfacesAndSelfTo<MouseInput>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<KeyboardInput>().AsSingle().NonLazy();

            Container.Bind<CameraView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraPanController>().AsSingle().NonLazy();

            Container.Bind<CanonView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CanonPresenter>().AsSingle().NonLazy();

        }

    }
}
