using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct CannonBallMovementSystem : ISystem
{


    public void OnCreate(ref SystemState state)
    {
        

    }

    public void OnDestroy(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (cannonBallLife, parabolaData, translation) in SystemAPI.Query<RefRW<CannonBall>,
                     RefRW<ParabolaData>, TransformAspect>())
        {
            var localTime = cannonBallLife.ValueRW + SystemAPI.Time.DeltaTime / parabolaData.ValueRO.duration;
            var newTrans = new float3(
                math.lerp(parabolaData.ValueRO.startPoint.x, parabolaData.ValueRO.endPoint.x, localTime),
                Parabola.Solve(parabolaData.ValueRW, localTime),
                math.lerp(parabolaData.ValueRW.startPoint.y, parabolaData.ValueRW.endPoint.y, localTime));

            translation.Position = newTrans;
            cannonBallLife.ValueRW = localTime;
        }


    }
}
