﻿using System;
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

        private int _reflectionsCount = 2;

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

            _collider
                .OnCollisionEnterAsObservable()
                .Subscribe(delegate(Collision collision)
                {
                    _reflectionsCount--;
                    velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
                    velocity *= _cannonConfig.ReflectionVelocityMultiplier;

                    if (_reflectionsCount == 0)
                    {
                        Dispose();

                        Quaternion explosionRotation = Quaternion.LookRotation(collision.contacts[0].normal, Vector3.up) * Quaternion.Euler(90, 0, 0);
                        Vector3 explosionPosition = collision.contacts[0].point - collision.contacts[0].normal * 0.1f;
                        CreateExplosion(explosionRotation, explosionPosition);
                        Destroy(gameObject);
                    }

                    if (collision.gameObject.TryGetComponent(out Wall wall))
                    {
                        wall.VisualizeHit(collision.contacts[0].point);
                    }
                });

            Observable
                .EveryUpdate()
                .Subscribe(delegate
                {
                    // movement
                    Vector3 position = transform.position;
                    position += velocity * deltaTime;
                    position.y -= 0.5f * Constants.Gravity * deltaTime * deltaTime;

                    transform.position = position;

                    // gravity;
                    velocity += Constants.Gravity * deltaTime * Vector3.down;
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
                    CreateExplosion(Quaternion.identity, transform.position);
                    Destroy(gameObject);
                }).AddTo(_compositeDisposable);
        }

        private void CreateExplosion(Quaternion rotation, Vector3 position)
        {
            Instantiate(_explosionEffect, position, rotation);
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
