using System.Collections;
using System.Collections.Generic;
#if UNITY_2019_1_OR_NEWER
using UnityEngine;
#endif

namespace VoronatorSharp
{
    public struct Triangle : IEnumerable<Vector2>
    {
        public int TriangleIndex;

        public Vector2 Point1;
        public Vector2 Point2;
        public Vector2 Point3;

        public Triangle(int triangleIndex, Vector2 point1, Vector2 point2, Vector2 point3)
        {
            TriangleIndex = triangleIndex;
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
        }

        public Vector2 Centroid => (Point1 + Point2 + Point3) / 3;
        public Vector2 Circumcenter => Delaunator.GetCircumcenter(Point1, Point2, Point3);

        public IEnumerator<Vector2> GetEnumerator()
        {
            yield return Point1;
            yield return Point2;
            yield return Point3;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Point1;
            yield return Point2;
            yield return Point3;
        }
    }
}
