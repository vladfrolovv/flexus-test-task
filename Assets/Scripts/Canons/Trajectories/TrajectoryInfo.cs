using UnityEngine;
namespace Canons.Trajectories
{
    public class TrajectoryInfo
    {
        public TrajectoryInfo(Vector3[] points)
        {
            Points = points;
        }

        public Vector3[] Points { get; }
    }
}
