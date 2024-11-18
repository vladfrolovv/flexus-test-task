using Cameras;
using Canons;
using Canons.Projectiles;
using Canons.Trajectories;
using Inputs;
using Zenject;
namespace Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {

            Container.BindInterfacesAndSelfTo<CanonInput>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<KeyboardInput>().AsSingle().NonLazy();

            Container.Bind<CameraView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraPanController>().AsSingle().NonLazy();

            Container.Bind<CanonView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CanonBarrelView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CanonPresenter>().AsSingle().NonLazy();

            Container.Bind<PowerSliderObserver>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ProjectileLaunchPoint>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<TrajectoryCalculator>().AsSingle().NonLazy();
        }

    }
}
