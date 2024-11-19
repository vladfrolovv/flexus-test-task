using System;
using Canons.Trajectories;
using Obstacles;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utilities;
using Zenject;
namespace Canons.CannonBalls
{
    public class Cannonball : MonoBehaviour, IPoolable<CannonballInfo, IMemoryPool>, IDisposable
    {

        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshCollider _collider;

        private CannonballInfo _info;
        private IMemoryPool _pool;

        private CanonConfig _canonConfig;
        private TrajectoryCalculator _trajectoryCalculator;

        [Inject]
        public void Construct(TrajectoryCalculator trajectoryCalculator, CanonConfig canonConfig)
        {
            _trajectoryCalculator = trajectoryCalculator;
            _canonConfig = canonConfig;
        }

        public void OnSpawned(CannonballInfo info, IMemoryPool pool)
        {
            _info = info;
            _pool = pool;

            _meshFilter.mesh = CannonballMeshGenerator.CreateCannonballMesh(
                new CannonballMeshInfo(_canonConfig.CannonballSize, _canonConfig.CannonballThickness));
            _collider.sharedMesh = _meshFilter.mesh;

            _collider.OnTriggerEnterAsObservable();
        }

        public void Launch()
        {
            Vector3 velocity = _trajectoryCalculator.Velocity;
            _collider.OnTriggerEnterAsObservable().Subscribe(delegate(Collider collision)
            {
                Vector3 hitPosition = collision.ClosestPoint(transform.position);
                Vector3 collisionNormal = (hitPosition - transform.position).normalized;
                velocity = Vector3.Reflect(velocity, collisionNormal) * _canonConfig.ReflectionVelocityMultiplier;

                if (collision.TryGetComponent(out Wall wall))
                {
                    wall.VisualizeHit(hitPosition);
                }
            });

            Observable.EveryUpdate().Subscribe(delegate
            {
                Vector3 position = transform.position;
                position += velocity * Time.deltaTime;
                position.y -= 0.5f * Constants.Gravity * Time.deltaTime * Time.deltaTime;

                velocity.y -= Constants.Gravity * Time.deltaTime;

                transform.position = position;
            });

            // _pool.Despawn(this);
        }

        public void OnDespawned()
        {
        }

        public void Dispose()
        {
        }

    }
}
