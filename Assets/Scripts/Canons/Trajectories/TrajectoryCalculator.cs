using System;
using Canons.Projectiles;
using UniRx;
using UnityEngine;
namespace Canons.Trajectories
{
    public class TrajectoryCalculator : IDisposable
    {

        private readonly CompositeDisposable _compositeDisposable = new();

        private float _startPosition;
        private float _trajectoryEndPosition;

        public ReactiveProperty<Vector2> Trajectory { get; } = new ();

        public TrajectoryCalculator(PowerSliderObserver powerSliderObserver, ProjectileLaunchPoint projectileLaunchPoint,
                                    CanonConfig canonConfig)
        {
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

    }
}
