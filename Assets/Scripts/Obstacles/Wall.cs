using System.Collections.Generic;
using Canons.CannonBalls;
using UnityEngine;
using Zenject;
namespace Obstacles
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Wall : MonoBehaviour
    {

        private CannonballHole _cannonballHolePrefab;

        private readonly List<CannonballHole> _holes = new();

        [Inject]
        public void Construct(CannonballHole cannonballHolePrefab)
        {
            _cannonballHolePrefab = cannonballHolePrefab;
        }

        public void VisualizeHit(Vector3 hitPosition)
        {
            hitPosition -= Vector3.forward * .5f;
            CannonballHole hole = Instantiate(_cannonballHolePrefab, hitPosition, Quaternion.identity, transform);
            hole.transform.localRotation = Quaternion.identity;
            _holes.Add(hole);
        }

        public void ClearAll()
        {
            _holes.ForEach(hole => Destroy(hole.gameObject));
        }
    }
}
