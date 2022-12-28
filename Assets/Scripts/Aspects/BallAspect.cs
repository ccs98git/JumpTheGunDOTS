using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

readonly partial struct BallAspect : IAspect
{
    public readonly RefRW<Ball> m_ball;
    public readonly TransformAspect transform;

    public int xGrid => m_ball.ValueRW.xGrid;
    public int yGrid => m_ball.ValueRW.yGrid;

    public int xGridGoal => m_ball.ValueRW.xGridGoal;
    public int yGridGoal => m_ball.ValueRW.yGridGoal;

    public int currentDirection => m_ball.ValueRW.currentDirection;
    public float3 Pos
    {
        get => transform.Position;
        set => transform.Position = value;
    }
}

