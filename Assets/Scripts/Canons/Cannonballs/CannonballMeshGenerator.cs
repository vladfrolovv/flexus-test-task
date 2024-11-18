using UnityEngine;
namespace Canons.CannonBalls
{
    public class CannonballMeshGenerator
    {

        public static Mesh CreateCannonballMesh(CannonballMeshInfo info)
        {
            Vector3[] vertices = {
                new (-RandomValue(), -RandomValue(),  RandomValue()),
                new ( RandomValue(), -RandomValue(),  RandomValue()),
                new ( RandomValue(),  RandomValue(),  RandomValue()),
                new (-RandomValue(),  RandomValue(),  RandomValue()),

                new (-RandomValue(), -RandomValue(), -RandomValue()),
                new ( RandomValue(), -RandomValue(), -RandomValue()),
                new ( RandomValue(),  RandomValue(), -RandomValue()),
                new (-RandomValue(),  RandomValue(), -RandomValue()),
            };

            int[] triangles = {
                0, 1, 2, 0, 2, 3,
                5, 4, 7, 5, 7, 6,
                4, 0, 3, 4, 3, 7,
                1, 5, 6, 1, 6, 2,
                3, 2, 6, 3, 6, 7,
                4, 5, 1, 4, 1, 0
            };

            Vector2[] uvs = {
                new (0, 0), new (1, 0), new (1, 1), new (0, 1),
                new (0, 0), new (1, 0), new (1, 1), new (0, 1),
            };

            Mesh mesh = new()
            {
                vertices = vertices,
                triangles = triangles,
                uv = uvs,
            };

            mesh.RecalculateNormals();

            return mesh;

            float RandomValue() => Random.Range(info.Size - info.Thickness, info.Size + info.Thickness);
        }

    }
}
