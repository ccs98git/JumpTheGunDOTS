using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[BurstCompile]
public partial struct CannonBallMovementSystem : ISystem
{


    public void OnCreate(ref SystemState state)
    {
        

    }

    public void OnDestroy(ref SystemState state)
    {

    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var cannonBallJob = new CannonBallJob
        {
            time = SystemAPI.Time.DeltaTime
        };
        state.Dependency =  cannonBallJob.Schedule(state.Dependency);
        state.Dependency.Complete();

    }
}
[BurstCompile]
partial struct CannonBallJob : IJobEntity
{
    public float time;
    void Execute(ref TimePar timerPar,ref Parabola parabolaData, ref TransformAspect translation)
    {
        var localTime = timerPar.parTime + time / parabolaData.duration;
        var newTrans = new float3(
            math.lerp(parabolaData.start.x, parabolaData.end.x, localTime),
            ParabolaSolve.Solve(parabolaData, localTime),
            math.lerp(parabolaData.start.y, parabolaData.end.y, localTime));

        translation.Position = newTrans;
        timerPar.parTime = localTime;
    }
}


[BurstCompile]
public partial struct DestroyCannonBalls : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnDestroy(ref SystemState state)
    {

    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //state.EntityManager.
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var (transPos, timerLife, entity) in SystemAPI.Query<CannonBall, RefRO<TimePar>>().WithAll<CannonBall>().WithEntityAccess())
        {
            if (timerLife.ValueRO.parTime >= 1.0f)
            {
                ecb.DestroyEntity(entity);
            }
        }
        //ecb.Dispose();


    }
}
