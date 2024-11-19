using System;
using UniRx;
using UnityEngine;
namespace Canons.CannonBalls
{
    public class ExplosionEffect : MonoBehaviour
    {

        protected void Awake()
        {
            Observable.Timer(TimeSpan.FromSeconds(1.2f))
                .Subscribe(delegate
                {
                    Destroy(gameObject);
                });
        }

    }
}
