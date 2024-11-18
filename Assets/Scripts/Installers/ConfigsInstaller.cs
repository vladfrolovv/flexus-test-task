using Cameras;
using UnityEngine;
using Zenject;
namespace Installers
{
    [CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "SO/ConfigsInstaller")]
    public class ConfigsInstaller : ScriptableObjectInstaller
    {

        [SerializeField] private CameraPanConfig _cameraPanConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_cameraPanConfig).AsSingle();
        }

    }
}
