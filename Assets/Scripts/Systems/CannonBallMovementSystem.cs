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

        foreach (var (life, parabolaData, translation) in SystemAPI.Query<RefRW<Life>,
                     RefRW<Parabola>, TransformAspect>())
        {
            var localTime = life.ValueRW.lifeTime + SystemAPI.Time.DeltaTime / parabolaData.ValueRO.duration;
            var newTrans = new float3(
                math.lerp(parabolaData.ValueRO.start.x, parabolaData.ValueRO.end.x, localTime),
                ParabolaSolve.Solve(parabolaData.ValueRW, localTime),
                math.lerp(parabolaData.ValueRW.start.y, parabolaData.ValueRW.end.y, localTime));

            translation.Position = newTrans;
            life.ValueRW.lifeTime = localTime;

            
        }





    }
}
