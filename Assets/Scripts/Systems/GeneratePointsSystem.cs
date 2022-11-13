using System.Drawing.Drawing2D;
using System.Drawing;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Reflection;
using Random = UnityEngine.Random;


// Unmanaged systems can be BurstCompiled, tag needs to be on the struct itself, as well as derived methods of ISystem
[BurstCompile]
partial struct GeneratePointsSystem : ISystem
{
    
    const int instancesPerBatch = 1023;
    // all ISystem derived methods need to be implemented, even if empty.
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
        
        
        List<Point> pointsList = new List<Point>();
        List<Bar> barsList = new List<Bar>();
        List<List<Matrix4x4>> matricesList = new List<List<Matrix4x4>>();
        matricesList.Add(new List<Matrix4x4>());
        Point[] points;
        Bar[] bars;
        int pointCount;

        Matrix4x4[][] matrices;
        MaterialPropertyBlock[] matProps;
        // buildings
        for (int i = 0; i < 35; i++)
        {
            int height = Random.Range(4, 12);
            Vector3 pos = new Vector3(Random.Range(-45f, 45f), 0f, Random.Range(-45f, 45f));
            float spacing = 2f;
            for (int j = 0; j < height; j++)
            {
                Point point = new Point();
                point.x = pos.x + spacing;
                point.y = j * spacing;
                point.z = pos.z - spacing;
                point.oldX = point.x;
                point.oldY = point.y;
                point.oldZ = point.z;
                if (j == 0)
                {
                    point.anchor = true;
                }
                pointsList.Add(point);
                point = new Point();
                point.x = pos.x - spacing;
                point.y = j * spacing;
                point.z = pos.z - spacing;
                point.oldX = point.x;
                point.oldY = point.y;
                point.oldZ = point.z;
                if (j == 0)
                {
                    point.anchor = true;
                }
                pointsList.Add(point);
                point = new Point();
                point.x = pos.x + 0f;
                point.y = j * spacing;
                point.z = pos.z + spacing;
                point.oldX = point.x;
                point.oldY = point.y;
                point.oldZ = point.z;
                if (j == 0)
                {
                    point.anchor = true;
                }
                pointsList.Add(point);
            }
        }

        // ground details
        for (int i = 0; i < 600; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-55f, 55f), 0f, Random.Range(-55f, 55f));
            Point point = new Point();
            point.x = pos.x + Random.Range(-.2f, -.1f);
            point.y = pos.y + Random.Range(0f, 3f);
            point.z = pos.z + Random.Range(.1f, .2f);
            point.oldX = point.x;
            point.oldY = point.y;
            point.oldZ = point.z;
            pointsList.Add(point);

            point = new Point();
            point.x = pos.x + Random.Range(.2f, .1f);
            point.y = pos.y + Random.Range(0f, .2f);
            point.z = pos.z + Random.Range(-.1f, -.2f);
            point.oldX = point.x;
            point.oldY = point.y;
            point.oldZ = point.z;
            if (Random.value < .1f)
            {
                point.anchor = true;
            }
            pointsList.Add(point);
        }

        int batch = 0;

        for (int i = 0; i < pointsList.Count; i++)
        {
            for (int j = i + 1; j < pointsList.Count; j++)
            {
                Bar bar = new Bar();
                AssignPoints(pointsList[i], pointsList[j], bar);
                if (bar.length < 5f && bar.length > .2f)
                {
                    bar.point1.neighborCount++;
                    bar.point2.neighborCount++;

                    barsList.Add(bar);
                    matricesList[batch].Add(bar.matrix);
                    if (matricesList[batch].Count == instancesPerBatch)
                    {
                        batch++;
                        matricesList.Add(new List<Matrix4x4>());
                    }
                    if (barsList.Count % 500 == 0)
                    {
                        //yield return null;
                        break;
                    }
                }
            }
        }
        points = new Point[barsList.Count * 2];
        pointCount = 0;
        for (int i = 0; i < pointsList.Count; i++)
        {
            if (pointsList[i].neighborCount > 0)
            {
                points[pointCount] = pointsList[i];
                pointCount++;
            }
        }
        Debug.Log(pointCount + " points, room for " + points.Length + " (" + barsList.Count + " bars)");

        bars = barsList.ToArray();

        matrices = new Matrix4x4[matricesList.Count][];
        for (int i = 0; i < matrices.Length; i++)
        {
            matrices[i] = matricesList[i].ToArray();
        }

        matProps = new MaterialPropertyBlock[barsList.Count];
        Vector4[] colors = new Vector4[instancesPerBatch];
        for (int i = 0; i < barsList.Count; i++)
        {
            colors[i % instancesPerBatch] = barsList[i].color;
            if ((i + 1) % instancesPerBatch == 0 || i == barsList.Count - 1)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                block.SetVectorArray("_Color", colors);
                matProps[i / instancesPerBatch] = block;
            }
        }

        pointsList = null;
        barsList = null;
        matricesList = null;
        System.GC.Collect();
        Time.timeScale = 1f;

    }


    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) { }

    [BurstCompile]
    public void AssignPoints(Point a, Point b, Bar bar)
    {
        bar.point1 = a;
        bar.point2 = b;
        Vector3 delta = new Vector3(bar.point2.x - bar.point1.x, bar.point2.y - bar.point1.y, bar.point2.z - bar.point1.z);
        bar.length = delta.magnitude;

        bar.thickness = UnityEngine.Random.Range(.25f, .35f);

        Vector3 pos = new Vector3(bar.point1.x + bar.point2.x, bar.point1.y + bar.point2.y, bar.point1.z + bar.point2.z) * .5f;
        Quaternion rot = Quaternion.LookRotation(delta);
        Vector3 scale = new Vector3(bar.thickness, bar.thickness, bar.length);
        bar.matrix = Matrix4x4.TRS(pos, rot, scale);

        bar.minX = Mathf.Min(bar.point1.x, bar.point2.x);
        bar.maxX = Mathf.Max(bar.point1.x, bar.point2.x);
        bar.minY = Mathf.Min(bar.point1.y, bar.point2.y);
        bar.maxY = Mathf.Max(bar.point1.y, bar.point2.y);
        bar.minZ = Mathf.Min(bar.point1.z, bar.point2.z);
        bar.maxZ = Mathf.Max(bar.point1.z, bar.point2.z);

        float upDot = Mathf.Acos(Mathf.Abs(Vector3.Dot(Vector3.up, delta.normalized))) / Mathf.PI;
        bar.color = UnityEngine.Color.white * upDot * UnityEngine.Random.Range(.7f, 1f);
    }
}