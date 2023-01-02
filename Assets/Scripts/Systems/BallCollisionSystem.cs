using System.Drawing;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine.UIElements;

public partial struct BallCollisionSystem : ISystem
{
    //private EntityCommandBufferSystem ECB;

    public void OnDestroy(ref SystemState state)
    {
        //throw new System.NotImplementedException();
    }

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Ball>();
    }
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var playerEntity = SystemAPI.GetSingletonEntity<Ball>();
        var playerBounds = SystemAPI.GetComponent<WorldRenderBounds>(playerEntity);
        var player = SystemAPI.GetAspectRW<BallAspect>(playerEntity);
        foreach (var cannonBall in SystemAPI.Query<TransformAspect>().WithAll<CannonBall>())
        {
            //float3 difference = player.Pos - cannonBall.Position;
            float distance = UnityEngine.Vector3.Distance(player.Pos, cannonBall.Position);
            if( distance < playerBounds.Value.Size.x)
            {
                //DIE Game over?
                ecb.DestroyEntity(playerEntity);
               
            }
        
        }
        
    }

    
}
