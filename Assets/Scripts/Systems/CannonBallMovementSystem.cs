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

        foreach (var (timerPar, parabolaData, translation) in SystemAPI.Query<RefRW<TimePar>,
                     RefRW<Parabola>, TransformAspect>())
        {
            var localTime = timerPar.ValueRW.parTime + SystemAPI.Time.DeltaTime / parabolaData.ValueRO.duration;
            var newTrans = new float3(
                math.lerp(parabolaData.ValueRO.start.x, parabolaData.ValueRO.end.x, localTime),
                ParabolaSolve.Solve(parabolaData.ValueRW, localTime),
                math.lerp(parabolaData.ValueRW.start.y, parabolaData.ValueRW.end.y, localTime));

            translation.Position = newTrans;
            timerPar.ValueRW.parTime = localTime;

            
        }





    }
}
public partial struct DestroyCannonBalls : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnDestroy(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (transPos, par, timerLife, entity) in SystemAPI.Query<CannonBall, RefRO<Parabola>, RefRO<TimePar>>().WithAll<CannonBall>().WithEntityAccess())
        {
            if (timerLife.ValueRO.parTime >= 1.0f)
            {
                ecb.DestroyEntity(entity);
            }
        }
        

    }
}
