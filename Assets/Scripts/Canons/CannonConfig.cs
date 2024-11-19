using UnityEngine;
namespace Canons
{
    [CreateAssetMenu(fileName = "CanonConfig", menuName = "SO/CanonConfig")]
    public class CannonConfig : ScriptableObject
    {

        [Header("Canon Movement")]
        [SerializeField] private float _maxElevation;
        [SerializeField] private float _minElevation;
        [SerializeField] private float _elevationSpeed;

        [Header("Canon Power")]
        [SerializeField] private float _powerStep;
        [SerializeField] private float _shotCooldown;
        [SerializeField] private float _speedMultiplier;

        [Header("Cannonball")]
        [SerializeField] private float _cannonballSize;
        [SerializeField] private float _cannonballThickness;
        [SerializeField] private float _reflectionVelocityMultiplier;
        [SerializeField] private float _selfDestructTime;
        [SerializeField] private float _velocityToDestroy;

        public float MaxElevation => _maxElevation;
        public float MinElevation => _minElevation;
        public float ElevationSpeed => _elevationSpeed;

        public float PowerStep => _powerStep;
        public float ShotCooldown => _shotCooldown;
        public float SpeedMultiplier => _speedMultiplier;

        public float CannonballSize => _cannonballSize;
        public float CannonballThickness => _cannonballThickness;
        public float ReflectionVelocityMultiplier => _reflectionVelocityMultiplier;
        public float SelfDestructTime => _selfDestructTime;
        public float VelocityToDestroy => _velocityToDestroy;


    }
}
