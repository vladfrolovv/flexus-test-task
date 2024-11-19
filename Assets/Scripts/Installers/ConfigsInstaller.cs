using Cameras;
using Canons;
using UnityEngine;
using Zenject;
namespace Installers
{
    [CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "SO/ConfigsInstaller")]
    public class ConfigsInstaller : ScriptableObjectInstaller
    {

        [SerializeField] private CameraPanConfig _cameraPanConfig;
        [SerializeField] private CannonConfig _cannonConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_cameraPanConfig).AsSingle();
            Container.BindInstance(_cannonConfig).AsSingle();
        }

    }
}
