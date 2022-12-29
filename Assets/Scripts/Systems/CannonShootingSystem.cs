using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.TerrainUtils;

public partial struct CannonShootingSystem : ISystem
{

    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Ball>();

    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
    
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingletonRW<Config>();
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var player = SystemAPI.GetSingletonEntity<Ball>();
        var playerAspect = SystemAPI.GetAspectRW<BallAspect>(player);
        foreach (var (cannon,translation) in SystemAPI.Query<RefRW<Cannon>,CannonAspect>().WithAll<Cannon>()) {
            cannon.ValueRW.coolDown += SystemAPI.Time.DeltaTime; 
            if (cannon.ValueRO.coolDown > 7.0f)
            {
                cannon.ValueRW.coolDown = UnityEngine.Random.Range(1, 7);
                var endPoint = playerAspect.transform;
                var startPoint = translation.transform.Position;
                var distance = new Vector2(endPoint.Position.z - startPoint.z, endPoint.Position.x + startPoint.x).magnitude;
                var duration = distance / 5.0f;
                if (duration < 0.0001)
                {
                    duration = 1;
                }

                var cannonBallEntity = ecb.Instantiate(cannon.ValueRO.CannonBall);
                ecb.SetComponent(cannonBallEntity, new Translation
                {
                    Value = translation.transform.Position
                });
                Parabola parabolaData = ParabolaSolve.Create(startPoint.y, config.ValueRO.maxHeight, endPoint.Position.y);
                parabolaData.start = startPoint.xz;
                parabolaData.end = endPoint.Position.xz;
                parabolaData.duration = duration;
                ecb.SetComponent(cannonBallEntity, parabolaData);
            }
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
        /*
        foreach (var (transPos, par, timerLife) in SystemAPI.Query<TransformAspect, RefRO<Parabola>, RefRO<Life>>().WithAll<CannonBall>())
        {
            if (timerLife.ValueRO.lifeTime >= par.ValueRO.duration)
            {
                transPos.Position = new float3(0, 20, 0);
            }
        }
        */

    }
}

