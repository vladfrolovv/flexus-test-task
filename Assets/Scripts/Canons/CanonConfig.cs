using UnityEngine;
namespace Canons
{
    [CreateAssetMenu(fileName = "CanonConfig", menuName = "SO/CanonConfig")]
    public class CanonConfig : ScriptableObject
    {

        [Header("Canon Movement")]
        [SerializeField] private float _maxElevation = 45f;
        [SerializeField] private float _minElevation;
        [SerializeField] private float _elevationSpeed = 1f;

        [Header("Canon Power")]
        [SerializeField] private float _powerStep = 1f;

        public float MaxElevation => _maxElevation;
        public float MinElevation => _minElevation;
        public float ElevationSpeed => _elevationSpeed;

        public float PowerStep => _powerStep;

    }
}
