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

        int xScale = 5;
        int yScale = 5;

        int groundMemAlloc = xScale * yScale;

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var terrain = CollectionHelper.CreateNativeArray<Entity>(groundMemAlloc, Allocator.Temp);
        ecb.Instantiate(config.Ground, terrain);


        int ix = 0;
        int iy = 0;

        foreach (Entity e in terrain) {

            
            LocalToWorldTransform ltwt = new LocalToWorldTransform();
            ltwt.Value.Position = new float3(ix, 1, 1);
            ltwt.Value.Scale = 1.0f;
            ecb.SetComponent(e , new LocalToWorldTransform
            {
                Value = ltwt.Value
            });

            ix++;
        }

        state.Enabled = false;
    }


}