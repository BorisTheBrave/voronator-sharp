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
    }
}