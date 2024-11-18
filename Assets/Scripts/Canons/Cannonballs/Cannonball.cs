using System;
using UnityEngine;
using Zenject;
namespace Canons.CannonBalls
{
    public class Cannonball : MonoBehaviour, IPoolable<CannonballInfo, IMemoryPool>, IDisposable
    {

        [SerializeField] private MeshFilter _meshFilter;

        private CannonballInfo _info;
        private IMemoryPool _pool;

        public void OnSpawned(CannonballInfo info, IMemoryPool pool)
        {
            _info = info;
            _pool = pool;

            _meshFilter.mesh = CannonballMeshGenerator.CreateCannonballMesh(new CannonballMeshInfo(.25f, .01f));
        }

        public void OnDespawned()
        {
        }

        public void Dispose()
        {
        }

    }
}
