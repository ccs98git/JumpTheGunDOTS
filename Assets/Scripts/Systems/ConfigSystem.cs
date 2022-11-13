using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static UnityEngine.GraphicsBuffer;

// Unmanaged systems can be BurstCompiled, tag needs to be on the struct itself, as well as derived methods of ISystem
[BurstCompile]
partial struct ConfigSystem : ISystem
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
        var config = SystemAPI.GetSingleton<Config>();

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var terrain = CollectionHelper.CreateNativeArray<Entity>(10, Allocator.Temp); // 10 is a static value, should be amount of ground
        ecb.Instantiate(config.Ground, terrain);

        state.Enabled = false;
    }


}