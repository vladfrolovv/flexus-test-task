using System;
using UnityEngine;
using Zenject;
namespace Canons.CannonBalls
{
    public class Cannonball : MonoBehaviour, IPoolable<CannonballInfo, IMemoryPool>, IDisposable
    {

        private CannonballInfo _info;
        private IMemoryPool _pool;

        public void OnSpawned(CannonballInfo info, IMemoryPool pool)
        {
            _info = info;
            _pool = pool;
        }

        public void OnDespawned()
        {
        }

        public void Dispose()
        {
        }

    }
}
