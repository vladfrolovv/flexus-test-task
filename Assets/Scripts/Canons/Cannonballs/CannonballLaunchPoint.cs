using UnityEngine;
namespace Canons.CannonBalls
{
    public class CannonballLaunchPoint : MonoBehaviour
    {

        [SerializeField] private Transform _directionPoint;

        public Vector3 Position => transform.position;

        public Vector3 Direction => (Position - _directionPoint.position).normalized;

    }
}
