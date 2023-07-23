using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace VoronatorSharp.Test
{
    [TestClass]
    public class VoronatorTest
    {
        [TestMethod]
        public void TestBasicVoronoi()
        {
            var points = new List<Vector2>(){
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
            };
            var v = new Voronator(points);
            CollectionAssert.AreEqual(
                new List<Vector2>()
                {
                    new Vector2(-1e-6f, -1e-6f),
                    new Vector2(0.5f, -1e-6f),
                    new Vector2(0.5f, 0.5f),
                    new Vector2(-1e-6f, 0.5f),
                },
                v.GetClippedPolygon(0)
                );
            CollectionAssert.AreEqual(
                new List<Vector2>()
                {
                    new Vector2(0.5f, 0.5f),
                },
                v.GetPolygon(0)
                );
            CollectionAssert.AreEqual(new[] { 1, 2 }, v.Neighbors(0).ToArray());
            CollectionAssert.AreEqual(new[] { 2, 0 }, v.Neighbors(1).ToArray());
            CollectionAssert.AreEqual(new[] { 0, 1 }, v.Neighbors(2).ToArray());
        }

        [TestMethod]
        public void TestDegenerateVoronoi()
        {
            // This is "degenerate" as more than three voronoi cells
            // are equidistant from (0.5, 0.5)
            // As the triangulation implies only three things meet at a point
            // this can lead to some unintuive behaviour.

            var points = new List<Vector2>(){
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1),
            };
            var v = new Voronator(points, new Vector2(0, 0), new Vector2(1, 1));

            CollectionAssert.AreEqual(
                new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(0.5f, 0),
                    new Vector2(0.5f, 0.5f),
                    new Vector2(0, 0.5f),
                },
                v.GetClippedPolygon(0)
                );
            CollectionAssert.AreEqual(
                new List<Vector2>()
                {
                    new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f),
                },
                v.GetPolygon(0)
                );
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, v.Neighbors(0).ToArray());
            CollectionAssert.AreEqual(new[] { 1, 3 }, v.ClippedNeighbors(0).ToArray());


            CollectionAssert.AreEqual(
                new List<Vector2>()
                {
                    new Vector2(0.5f, 0.5f),
                },
                v.GetPolygon(1)
                );
            CollectionAssert.AreEqual(new[] { 2, 0 }, v.Neighbors(1).ToArray());
            CollectionAssert.AreEqual(new[] { 2, 0 }, v.ClippedNeighbors(1).ToArray());
        }

        [TestMethod]
        public void TestOnePointDiagram()
        {
            var points = new List<Vector2>(){
                new Vector2(0, 0),
            };
            var v = new Voronator(points, new Vector2(-2, -1), new Vector2(2, 1));
            CollectionAssert.AreEqual(
                new List<Vector2>()
                {
                    new Vector2(2, -1),
                    new Vector2(2, 1),
                    new Vector2(-2, 1),
                    new Vector2(-2, -1),
                },
                v.GetClippedPolygon(0)
                );
            CollectionAssert.AreEqual(new int[0], v.Neighbors(0).ToArray());
            CollectionAssert.AreEqual(new int[0], v.ClippedNeighbors(0).ToArray());
        }

        [TestMethod]
        public void TestTwoPointDiagram()
        {
            var points = new List<Vector2>(){
                new Vector2(-1, 0),
                new Vector2(1, 0),
            };
            var v = new Voronator(points, new Vector2(-2, -1), new Vector2(2, 1));
            CollectionAssert.AreEqual(
                new List<Vector2>()
                {
                    new Vector2(-2, 1),
                    new Vector2(-2, -1),
                    new Vector2(0, -1),
                    new Vector2(0, 1),
                },
                v.GetClippedPolygon(0)
                );
            CollectionAssert.AreEqual(new[] { 1 }, v.Neighbors(0).ToArray());
            CollectionAssert.AreEqual(new[] { 1 }, v.ClippedNeighbors(0).ToArray());
        }

        [TestMethod]
        public void TestCollinearPointDiagram()
        {
            var points = new List<Vector2>(){
                new Vector2(-1, 0),
                new Vector2(0, 0),
                new Vector2(1, 0),
            };
            var v = new Voronator(points, new Vector2(-2, -1), new Vector2(2, 1));


            CollectionAssert.AreEqual(
                new List<Vector2>()
                {
                    new Vector2(-2, 1),
                    new Vector2(-2, -1),
                    new Vector2(-0.5f, -1),
                    new Vector2(-0.5f, 1),
                },
                v.GetClippedPolygon(0)
                );
            CollectionAssert.AreEqual(new[] { 1 }, v.Neighbors(0).ToArray());
            CollectionAssert.AreEqual(new[] { 1 }, v.ClippedNeighbors(0).ToArray());

            var p = v.GetClippedPolygon(1);
            CollectionAssert.AreEqual(
                new List<Vector2>()
                {
                    new Vector2(-0.5f, 1),
                    new Vector2(-0.5f, -1),
                    new Vector2(0.5f, -1),
                    new Vector2(0.5f, 1),
                },
                v.GetClippedPolygon(1)
                );
            CollectionAssert.AreEqual(new[] { 0, 2 }, v.Neighbors(1).ToArray());
            CollectionAssert.AreEqual(new[] { 0, 2 }, v.ClippedNeighbors(1).ToArray());
        }

        [TestMethod]
        public void TestCollinearPointDiagram2()
        {
            var points = new List<Vector2>(){
                new Vector2(-1, -1),
                new Vector2(0, 0),
                new Vector2(1, 1),
            };
            var v = new Voronator(points, new Vector2(-1, -1), new Vector2(1, 1));

            Assert.AreEqual(6, v.GetClippedPolygon(1).Count);
            var p = v.GetClippedPolygon(1);
        }

        [TestMethod]
        public void TestDuplicatePoints()
        {
            List<Vector2> points = new List<Vector2>
            {
                new Vector2(8.481889f, -10.29803f),
                new Vector2(14.8841f, -11.41214f),
                new Vector2(17.28493f, -16.59971f),
                new Vector2(11.40464f, -14.75447f),
                new Vector2(4.44571f, -21.02133f),
                new Vector2(14.29259f, -22.76213f),
                new Vector2(3.437411f, -9.220477f),//dup
                new Vector2(17.73516f, -7.305126f),
                new Vector2(21.68072f, -22.66702f),
                new Vector2(13.86774f, -26.88861f),
                new Vector2(2.343594f, -24.34784f),
                new Vector2(3.437411f, -9.220477f)//dup
            };

            var v = new VoronatorSharp.Voronator(points);
            for (var i = 0; i < points.Count; i++)
            {
                var vertices = v.GetClippedPolygon(i);
            }
            Assert.AreEqual(null, v.GetClippedPolygon(11));
        }
    }
}