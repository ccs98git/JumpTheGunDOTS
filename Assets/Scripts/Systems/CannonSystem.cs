using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

// Unmanaged systems can be BurstCompiled, tag needs to be on the struct itself, as well as derived methods of ISystem
[BurstCompile]
partial struct CannonSystem : ISystem
{

    // all ISystem derived methods need to be implemented, even if empty.
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    { }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingletonRW<Config>();
        if (config.ValueRO.setupStage > 1) {
            // Turn Cannons to Player object
            foreach (var cannonE in SystemAPI.Query<CannonAspect>())
            {
                // -- Need a vector from cannon point to player transform. (only x and z value is nessecary. Dont wanna rotate on y)

                cannonE.transform.Rotation = Quaternion.identity;
            }
        }


        




    }
}