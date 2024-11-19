using Cameras;
using Canons;
using Canons.CannonBalls;
using Canons.Trajectories;
using Inputs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {

        [SerializeField] private Cannonball _cannonballPrefab;
        [SerializeField] private CannonballHole _cannonballHolePrefab;
        [SerializeField] private ExplosionEffect _explosionEffect;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CanonInput>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<KeyboardInput>().AsSingle().NonLazy();

            Container.Bind<CameraView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CameraShakeEffect>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraPanController>().AsSingle().NonLazy();

            Container.Bind<CanonView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CanonBarrelView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CanonPresenter>().AsSingle().NonLazy();

            Container.Bind<PowerSliderObserver>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CannonballLaunchPoint>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<TrajectoryCalculator>().AsSingle().NonLazy();

            Container.Bind<GraphicRaycaster>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<InputBlocker>().AsSingle().NonLazy();

            Container.BindInstance(_cannonballHolePrefab);

            InstallPrefabs();
        }

        private void InstallPrefabs()
        {
            Container.BindFactory<CannonballInfo, Cannonball, CannonballFactory>()
                .FromPoolableMemoryPool<CannonballInfo, Cannonball, CannonballPool>(x =>
                    x.WithInitialSize(24).FromComponentInNewPrefab(_cannonballPrefab));
        }

        private class CannonballPool : MonoPoolableMemoryPool<CannonballInfo, IMemoryPool, Cannonball>
        {
        }

    }
}
