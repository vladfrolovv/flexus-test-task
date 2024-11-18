using UnityEngine;
namespace Canons
{
    [CreateAssetMenu(fileName = "CanonConfig", menuName = "SO/CanonConfig")]
    public class CanonConfig : ScriptableObject
    {

        [SerializeField] private float _maxElevation = 45f;
        [SerializeField] private float _minElevation = 0f;
        [SerializeField] private float _elevationSpeed = 1f;
        [SerializeField] private float _rotationSpeed;

        public float MaxElevation => _maxElevation;
        public float MinElevation => _minElevation;

        public float ElevationSpeed => _elevationSpeed;

    }
}
