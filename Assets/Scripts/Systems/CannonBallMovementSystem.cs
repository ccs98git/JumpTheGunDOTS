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
                     RefRW<Parabola>, TransformAspect>())
        {
            var localTime = cannonBallLife.ValueRW.lifeLived.lifeTime + SystemAPI.Time.DeltaTime / parabolaData.ValueRO.duration;
            var newTrans = new float3(
                math.lerp(parabolaData.ValueRO.start.x, parabolaData.ValueRO.end.x, localTime),
                ParabolaSolve.Solve(parabolaData.ValueRW, localTime),
                math.lerp(parabolaData.ValueRW.start.y, parabolaData.ValueRW.end.y, localTime));

            translation.Position = newTrans;
            cannonBallLife.ValueRW.lifeLived.lifeTime = localTime;
        }


    }
}
