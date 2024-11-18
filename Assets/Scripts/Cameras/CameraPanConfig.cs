using UnityEngine;
namespace Cameras
{
    [CreateAssetMenu(fileName = "CameraPanConfig", menuName = "SO/CameraPanConfig")]
    public class CameraPanConfig : ScriptableObject
    {

        [SerializeField] private float _panSpeed = 1f;
        [SerializeField] private float _maxYaw = 10f;

        public float PanSpeed => _panSpeed;
        public float MaxYaw => _maxYaw;

    }
}
