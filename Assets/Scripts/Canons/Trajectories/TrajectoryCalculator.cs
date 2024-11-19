using System;
using Canons.CannonBalls;
using Inputs;
using UniRx;
using UnityEngine;
using Utilities;
namespace Canons.Trajectories
{
    public class TrajectoryCalculator : IDisposable
    {

        private const int Resolution = 32;

        private readonly CannonballLaunchPoint _cannonballLaunchPoint;
        private readonly PowerSliderObserver _powerSliderObserver;
        private readonly CannonConfig _cannonConfig;

        private readonly CompositeDisposable _compositeDisposable = new();

        private readonly ReactiveProperty<TrajectoryInfo> _trajectory = new ();
        public IReadOnlyReactiveProperty<TrajectoryInfo> Trajectory => _trajectory;

        public Vector3 Velocity =>
            _cannonballLaunchPoint.Direction * _powerSliderObserver.Power.Value * _cannonConfig.PowerStep;

        public TrajectoryCalculator(PowerSliderObserver powerSliderObserver, KeyboardInput keyboardInput, CannonballLaunchPoint cannonballLaunchPoint,
                                    CannonConfig cannonConfig)
        {
            _cannonballLaunchPoint = cannonballLaunchPoint;
            _powerSliderObserver = powerSliderObserver;
            _cannonConfig = cannonConfig;

            powerSliderObserver.Power.Subscribe(delegate
            {
                RecalculateTrajectory();
            }).AddTo(_compositeDisposable);

            keyboardInput.Direction.Subscribe(delegate
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
            Vector3 startPosition = _cannonballLaunchPoint.Position;
            Vector3 direction = _cannonballLaunchPoint.Direction;

            Vector3 velocity = direction * power * _cannonConfig.PowerStep;
            Vector3 horizontalVelocity = new (velocity.x, 0, velocity.z);
            float verticalVelocity = velocity.y;

            Vector3[] trajectoryPoints = new Vector3[Resolution];
            for (int i = 0; i < Resolution; i++)
            {
                float time = i * 0.1f;

                Vector3 horizontalDisplacement = horizontalVelocity * time;

                float verticalDisplacement = (verticalVelocity * time) - (0.5f * Constants.Gravity * time * time);
                trajectoryPoints[i] = startPosition + horizontalDisplacement + new Vector3(0, verticalDisplacement, 0);
            }
            return trajectoryPoints;
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

    }
}
