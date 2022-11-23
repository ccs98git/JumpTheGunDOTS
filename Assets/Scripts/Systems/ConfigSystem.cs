using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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

        // -- Scale x and y, overwrite to scale up of need be --
        int xScale = config.xScale;
        int yScale = config.yScale;
        // -----------------------------------------------------

        int groundMemAlloc = xScale * yScale;

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var terrain = CollectionHelper.CreateNativeArray<Entity>(groundMemAlloc, Allocator.Temp);
        ecb.Instantiate(config.Ground, terrain);


        int ix = 0;
        int iy = 0;
        float heightIndex = 0; // <- random height, multiplied by magnitude 2.0f

        foreach (Entity e in terrain) {

            int rand = UnityEngine.Random.Range(1, 10);
            heightIndex = 0.2f * rand; // <- random int between 1 and 9 (inclusive)


            // Transform Manipulation, place and scale.
            float3 Position = new float3(ix, (heightIndex / 2.0f), iy);
            float3 newScale = new float3(1, 1+heightIndex, 1);
            
            ecb.SetComponent(e, new Translation
            {
                Value = Position
            });
            ecb.AddComponent(e, new NonUniformScale
            {
                Value = newScale
            });



            // Color Generation based on height:
            float heightColor = 85f;
            for (int i = rand; i > 0; i--)
            {
                heightColor -= 8f;
            }
            Vector3 flatColor = new Vector3(heightColor, 202f, 56f).normalized; //<- not sure if/why this works, or if it works as intended.
            Color newColor = UnityEngine.Color.HSVToRGB(flatColor.x, flatColor.y, flatColor.z);

            ecb.SetComponent(e, new URPMaterialPropertyBaseColor
            {
                Value = (UnityEngine.Vector4)newColor
            });



            // Set height in the Ground Component
            ecb.SetComponent(e, new Ground
            {
                height = rand // <- assignment of height index
            });

            


            if (ix % (xScale-1) == 0 && ix != 0) { iy++; ix = 0; }
            else ix++;
        }
        state.Enabled = false;
    }


}