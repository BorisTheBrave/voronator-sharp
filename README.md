# VoronatorSharp

VoronatorSharp is a C# library that computes [Voronoi diagrams](https://en.wikipedia.org/wiki/Voronoi_diagram). The Voronoi diagram for a collection of points is the polygons that enclose the areas nearest each of those sites.

Voronoi diagrams have applications in a number of areas such as computer graphics.

This library features:
 * Computes Voronoi diagrams and [Delaunay triangulations](https://en.wikipedia.org/wiki/Delaunay_triangulation).
 * Voronoi polygons can be clipped to a rectangular area.
 * Uses a `n log(n)` [sweephull algorithm](https://github.com/mapbox/delaunator#papers).
 * The implementation attempts to minimize memory allocations.
 * Integrates with Unity or can be be used standalone.
 * Uses [robust orientation code](https://github.com/govert/RobustGeometry.NET).

 # Credits

The majority of this code is adapted from:
* [delaunator-sharp](https://github.com/nol1fe/delaunator-sharp) - computes Delaunay triangulation
* [d3-delaunay](https://github.com/d3/d3-delaunay) - clip polygons to a rectangle
* [RobustGeometry.NET](https://github.com/govert/RobustGeometry.NET) - robust geometry predicates

All code is licensed on MIT-like terms.

# Installation

Currently this is not listed on NuGet nor available as a Unity package. Please contact me if these interest you.

You can find pre-compiled dlls on the GitHub release page, these can be directly refenced by other projects and have no dependencies.

For Unity, copy the pre-compiled *for Unity* dll into your project, or copy the source code itself.

# Usage

## Voronator

The Voronator class computes a Voronoi diagram for a set of points.

```csharp
var points = new Vector[]{ new Vector(0, 0), new Vector(0, 1), new Vector(1, 0)};
var v = new VoronatorSharp.Voronator(points);
for (var i=0; i < points.Length; i++)
{
    var vertices = v.GetClippedPolygon(i);
}
```

Voronoi cells on the outside of the diagram can be *unbounded* meaning they extend off to infinity, and may have less than 3 vertices. 
For this reason, some methods come in a "clipped" and "unclipped" variety. 
A clipped method works with voronoi cells that have been cut to fit inside a rectangle called the clipping rectangle.

The clipping rectangle is by default large enough to cover all bounded cells, but 
can be set to anything in the constructor.

Clipped methods also deal better with degenerate cases such as many cells sharing the same vertex, so are recommended in most cases,
despite being slightly slower.

### Methods

The key methods are documented below - there are further methods and comments in the source.

```csharp
public List<Vector2> Voronator.GetPolygon(int i)
```
Returns the vertices of the voronoi cell, without any clipping.


```csharp
public bool Voronator.GetPolygon(int i, List<Vector2> vertices, out Vector2 ray1, out Vector2 ray2)
```
A version of GetPolygon that avoids allocating memory for vertices, and can return the rays associated with an unbounded cell.


```csharp
public List<Vector2> Voronator.GetClippedPolygon(int i)
```
Returns the vertices of the voronoi cell i after clipping to the clipping rectangle.
Returns null if the polygon is fully outside the clipping rectangle.


```csharp
public IEnumerable<int> Voronator.Neighbors(int i)
```
Returns the Voronoi cells that border the given cell.
This ignores clipping and may return odd results in some degenerate cases.

```csharp
public IEnumerable<int> Voronator.Neighbors(int i)
```
Returns the Voronoi cells that border the given cell inside the clipping rectangle.

```csharp
public int Voronator.Find(Vector2 u, int i = 0)
```
Finds the Voronoi cell that contains the given point, or equivalently,
finds the point that is nearest the given point.
This ignores clipping, so it always succeeds.

* *u* - The point to search for.
* *i* - Optional, the voronoi cell to start the search at. Useful if you know the returned cell will be nearby.


```csharp
public List<Vector2> Voronator.GetRelaxedPoints()
```
Returns the centroid of each voronoi cell.
This is suitable for use with Lloyd relaxation.
Unbounded cells are clipped down, which tends to move them inwards.

## Delaunator

The Delaunator class computes a Delaunay triangulation for a set of points.
The API similar to [DelaunatorSharp](https://github.com/nol1fe/delaunator-sharp), and you can find a helpful description of this datastructure [here](https://mapbox.github.io/delaunator/).

### Methods

The key methods are documented below - there are further methods and comments in the source.

```csharp
public IEnumerable<Triangle> Delaunator.GetTriangles()
```
Returns the points of all triangles in the Delauney triangulation.
A `Triangle` has the vector location of exactly 3 points, `Point1`, `Point2` and `Point3`, and properties for computing the `Centroid` and `Circumcenter`.


```csharp
public IEnumerable<(Vector2, Vector2)> Delaunator.GetEdges()
```
Returns all edges in the triangulation.
Each edge is only represented once, even if there is a triangle on either side.

```csharp
public int[] Delaunator.Triangles { get; }
```
One value per half-edge, containing the point index of where a given half edge starts.

```csharp
public int[] Delaunator.Halfedges { get; }
```
One value per half-edge, containing the opposite half-edge in the adjacent triangle, or -1 if there is no adjacent triangle

```csharp
public Vector2[] Delaunator.Points { get; }
```
The initial points Delaunator was constructed with.
