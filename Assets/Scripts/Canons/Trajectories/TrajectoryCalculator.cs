using System;
using Canons.Projectiles;
using Inputs;
using UniRx;
using UnityEngine;
namespace Canons.Trajectories
{
    public class TrajectoryCalculator : IDisposable
    {

        private const float Gravity = 9.81f;
        private const int Resolution = 32;

        private readonly ProjectileLaunchPoint _projectileLaunchPoint;
        private readonly PowerSliderObserver _powerSliderObserver;
        private readonly CanonConfig _canonConfig;

        private readonly CompositeDisposable _compositeDisposable = new();

        private readonly ReactiveProperty<TrajectoryInfo> _trajectory = new ();
        public IReadOnlyReactiveProperty<TrajectoryInfo> Trajectory => _trajectory;

        public TrajectoryCalculator(PowerSliderObserver powerSliderObserver, KeyboardInput keyboardInput, ProjectileLaunchPoint projectileLaunchPoint,
                                    CanonConfig canonConfig)
        {
            _projectileLaunchPoint = projectileLaunchPoint;
            _powerSliderObserver = powerSliderObserver;
            _canonConfig = canonConfig;

            powerSliderObserver.Power.Subscribe(delegate(float power)
            {
                RecalculateTrajectory();
            }).AddTo(_compositeDisposable);

            keyboardInput.Direction.Subscribe(delegate(Vector2Int direction)
            {
                RecalculateTrajectory();
            }).AddTo(_compositeDisposable);
        }

        private void RecalculateTrajectory()
        {
            _trajectory.Value = new TrajectoryInfo(GetTrajectoryPoints(_powerSliderObserver.Power.Value));
        }

        private Vector3[] GetTrajectoryPoints(float power)
        {
            Vector3 startPosition = _projectileLaunchPoint.Position;
            Vector3 direction = _projectileLaunchPoint.Direction;

            Vector3 velocity = direction * power * _canonConfig.PowerStep;
            Vector3 horizontalVelocity = new (velocity.x, 0, velocity.z);
            float verticalVelocity = velocity.y;

            Vector3[] trajectoryPoints = new Vector3[Resolution];
            for (int i = 0; i < Resolution; i++)
            {
                float time = i * 0.1f;

                Vector3 horizontalDisplacement = horizontalVelocity * time;

                float verticalDisplacement = (verticalVelocity * time) - (0.5f * Gravity * time * time);
                trajectoryPoints[i] = startPosition + horizontalDisplacement + new Vector3(0, verticalDisplacement, 0);
            }
            return trajectoryPoints;
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

    }
}
