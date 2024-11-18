using System;
using UniRx;
using UnityEngine;
using Zenject;
namespace Canons.Trajectories
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryView : MonoBehaviour, IDisposable
    {

        private LineRenderer _lineRenderer;
        private TrajectoryCalculator _trajectoryCalculator;

        [Inject]
        public void Construct(TrajectoryCalculator trajectoryCalculator)
        {
            _trajectoryCalculator = trajectoryCalculator;
        }

        protected void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _trajectoryCalculator.Trajectory.Subscribe(delegate(TrajectoryInfo info)
            {
                _lineRenderer.positionCount = info.Points.Length;
                _lineRenderer.SetPositions(info.Points);
            });
        }

        public void Dispose()
        {
        }

    }
}
