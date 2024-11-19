using UnityEngine;
namespace Canons
{
    public class CannonView : MonoBehaviour
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
