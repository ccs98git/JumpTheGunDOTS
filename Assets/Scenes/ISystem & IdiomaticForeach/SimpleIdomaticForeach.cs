using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct SimpleIdomaticForeach : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DoNotMoveTag>();
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }

    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
       
       foreach (var localworldTransform in SystemAPI.Query<RefRW<LocalToParentTransform>>())
       {
           localworldTransform.ValueRW.Value = localworldTransform.ValueRO.Value.Translate(new float3(0, 0, 0.01f));
       }

       foreach (var (lwt, speed) in SystemAPI.Query<RefRW<LocalToWorldTransform>, RefRO<IFESpeed>>().WithNone<DoNotMoveTag>())
       {
           lwt.ValueRW.Value = lwt.ValueRO.Value.RotateZ(speed.ValueRO.Value * dt);
       }
    }
}
