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

        [Header("Cannonball")]
        [SerializeField] private float _cannonballSize = .25f;
        [SerializeField] private float _cannonballThickness = .1f;
        [SerializeField] private float _reflectionVelocityMultiplier = .25f;
        [SerializeField] private float _selfDestructTime = 2.5f;
        [SerializeField] private float _velocityToDestroy = 12.5f;

        public float MaxElevation => _maxElevation;
        public float MinElevation => _minElevation;
        public float ElevationSpeed => _elevationSpeed;

        public float PowerStep => _powerStep;

        public float CannonballSize => _cannonballSize;
        public float CannonballThickness => _cannonballThickness;

        public float ReflectionVelocityMultiplier => _reflectionVelocityMultiplier;
        public float SelfDestructTime => _selfDestructTime;
        public float VelocityToDestroy => _velocityToDestroy;


    }
}
