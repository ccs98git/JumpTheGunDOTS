using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public partial struct AspectExampleSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<moveSpeed>();
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }

    public void OnUpdate(ref SystemState state)
    {
        float elapsedTime = (float)SystemAPI.Time.ElapsedTime;

        foreach (var sin in SystemAPI.Query<SinAspect>())
        {
            sin.Move(elapsedTime);
            
            
        }
    }
}


readonly partial struct SinAspect : IAspect
{
    //readonly RefRW<LocalToWorldTransform> m_Transform;
    readonly RefRO<moveSpeed> m_Speed;

    public void Move(double elapsedTime)
    {
        //m_Transform.ValueRW.Value.Position.y = (float)math.sin(elapsedTime * m_Speed.ValueRO.Value);
    }
}