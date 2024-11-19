using System;
using System.Collections;
using Canons.Trajectories;
using UnityEngine;
using Utilities;
using Zenject;
namespace Canons.CannonBalls
{
    public class Cannonball : MonoBehaviour, IPoolable<CannonballInfo, IMemoryPool>, IDisposable
    {

        [SerializeField] private MeshFilter _meshFilter;

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
        }

        public void Launch()
        {
            StartCoroutine(MoveProjectile());
        }

        private IEnumerator MoveProjectile()
        {
            Vector3 startPosition = transform.position;
            Vector3 position = startPosition;

            Vector3 velocity = _trajectoryCalculator.Velocity;

            float time = 0;
            while (position.y >= 0)
            {
                position.x = startPosition.x + velocity.x * time;
                position.z = startPosition.z + velocity.z * time;
                position.y = startPosition.y + velocity.y * time - 0.5f * Constants.Gravity * time * time;

                transform.position = position;

                time += Time.deltaTime;

                yield return null;
            }

            _pool.Despawn(this);
        }

        public void OnDespawned()
        {
        }

        public void Dispose()
        {
        }

    }
}
