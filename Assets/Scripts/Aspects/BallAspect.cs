using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

readonly partial struct BallAspect : IAspect
{
    readonly RefRW<Ball> m_ball;
    public readonly TransformAspect transform;
    public float3 Pos
    {
        get => transform.Position;
        set => transform.Position = value;
    }
}

