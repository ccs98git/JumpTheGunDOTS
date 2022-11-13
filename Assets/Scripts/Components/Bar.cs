using Unity.Entities;
using System.Collections;
using UnityEngine; // <- memory hit?

// Component: Bar
struct Bar : IComponentData
{
    public Point point1;
    public Point point2;
    public float length;
    public Matrix4x4 matrix;
    public float oldDX;
    public float oldDY;
    public float oldDZ;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
    public Color color;
    public float thickness;
}
