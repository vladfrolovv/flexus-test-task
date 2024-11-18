using UnityEngine;
namespace Cameras
{
    public class CameraView : MonoBehaviour
    {

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

    }
}
