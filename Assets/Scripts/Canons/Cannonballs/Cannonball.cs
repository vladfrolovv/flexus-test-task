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

        private readonly CompositeDisposable _compositeDisposable = new ();

        private CannonballInfo _info;
        private IMemoryPool _pool;

        private CannonConfig _cannonConfig;
        private ExplosionEffect _explosionEffect;
        private TrajectoryCalculator _trajectoryCalculator;

        [Inject]
        public void Construct(TrajectoryCalculator trajectoryCalculator, CannonConfig cannonConfig, ExplosionEffect explosionEffect)
        {
            _trajectoryCalculator = trajectoryCalculator;
            _cannonConfig = cannonConfig;
            _explosionEffect = explosionEffect;
        }

        public void OnSpawned(CannonballInfo info, IMemoryPool pool)
        {
            _info = info;
            _pool = pool;

            _meshFilter.mesh = CannonballMeshGenerator.CreateCannonballMesh(
                new CannonballMeshInfo(_cannonConfig.CannonballSize, _cannonConfig.CannonballThickness));
            _collider.sharedMesh = _meshFilter.mesh;
        }

        public void Launch()
        {
            Vector3 velocity = _trajectoryCalculator.Velocity;
            float deltaTime = Time.deltaTime * _cannonConfig.SpeedMultiplier;
            Observable.EveryUpdate().Subscribe(delegate
            {
                // movement
                Vector3 position = transform.position;
                position += velocity * deltaTime;
                position.y -= 0.5f * Constants.Gravity * deltaTime * deltaTime;

                transform.position = position;

                // gravity;
                velocity += Constants.Gravity * deltaTime * Vector3.down;
            }).AddTo(_compositeDisposable);

            _collider.OnTriggerEnterAsObservable().Subscribe(delegate(Collider collision)
            {
                if (velocity.magnitude < _cannonConfig.VelocityToDestroy)
                {
                    Dispose();
                    CreateExplosion(collision.transform.rotation);
                    Destroy(gameObject);
                }
                Vector3 hitPosition = collision.ClosestPoint(transform.position);
                Vector3 collisionNormal = (hitPosition - (transform.position - velocity * 5f)).normalized;
                velocity = Vector3.Reflect(velocity, collisionNormal) * _cannonConfig.ReflectionVelocityMultiplier;

                if (collision.TryGetComponent(out Wall wall))
                {
                    wall.VisualizeHit(hitPosition);
                }
            }).AddTo(_compositeDisposable);

            SelfDestruct();
        }

        private void SelfDestruct()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(_cannonConfig.SelfDestructTime))
                .Subscribe(delegate
                {
                    Dispose();
                    CreateExplosion(Quaternion.identity);
                    Destroy(gameObject);
                }).AddTo(_compositeDisposable);
        }

        private void CreateExplosion(Quaternion rotation)
        {
            Instantiate(_explosionEffect, transform.position, rotation);
        }

        public void OnDespawned()
        {
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable?.Clear();
        }

    }
}
