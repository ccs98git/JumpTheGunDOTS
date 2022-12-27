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
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var playerTranslation = SystemAPI.GetComponent<Translation>(SystemAPI.GetSingletonEntity<Ball>());
        foreach(var (cannon,translation) in SystemAPI.Query<RefRW<Cannon>,RefRW<Translation>>().WithAll<Cannon>()) {
            cannon.ValueRW.coolDown += SystemAPI.Time.DeltaTime;
            if (cannon.ValueRO.coolDown > 10.0f)
            {
                cannon.ValueRW.coolDown = 0;
                var endPoint = playerTranslation.Value;
                var startPoint = translation.ValueRO.Value;
                var distance = new Vector2(endPoint.z - startPoint.z, endPoint.x + startPoint.x).magnitude;
                var duration = distance / 5.0f;
                if (duration < 0.0001)
                {
                    duration = 1;
                }

                var bulletEntity = ecb.Instantiate(cannon.ValueRO.CannonBall);
                ecb.SetComponent(bulletEntity, new Translation
                {
                    Value = translation.ValueRO.Value
                });
                float3 parabolaData = Parabola.Create(startPoint.x, 5, endPoint.y);
                //parabolaData.x = startPoint.xz;
                //parabolaData.y = endPoint.xz;
                //parabolaData.z = duration;
                //ecb.SetComponent(bulletEntity, parabolaData);
            }
        }
       
        
    }
}

