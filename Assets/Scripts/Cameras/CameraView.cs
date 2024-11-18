using UnityEngine;
namespace Cameras
{
    public class CameraView : MonoBehaviour
    {

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

    }
}
