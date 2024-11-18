using UnityEngine;
namespace Canons.Projectiles
{
    public class ProjectileLaunchPoint : MonoBehaviour
    {

        [SerializeField] private Transform _directionPoint;

        public Vector3 Position => transform.position;

        public Vector3 Direction => (Position - _directionPoint.position).normalized;

    }
}
