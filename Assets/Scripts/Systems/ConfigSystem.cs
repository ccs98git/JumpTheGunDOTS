using System.Diagnostics;
using System.Linq;
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
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();

        int xScale = 5;
        int yScale = 5;

        int groundMemAlloc = xScale * yScale;

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var terrain = CollectionHelper.CreateNativeArray<Entity>(groundMemAlloc * 2, Allocator.Temp);

        ecb.Instantiate(config.Ground, terrain);
        
        bool[,] currentLayer = new bool[xScale, yScale];
        int ix = 0;
        int iy = 0;
        int ix2 = 0;
        int iy2 = 0;

        foreach (Entity e in terrain) {
            LocalToWorldTransform ltwt = new LocalToWorldTransform();
            ltwt.Value.Position = new float3(ix, 1, iy);
            ltwt.Value.Scale = 1.0f;


            if (iy > 4)
            {

                ltwt.Value.Position = new float3(ix2, 1.2f, iy2);
                ltwt.Value.Scale = 1.0f;

                if (currentLayer[ix2, iy2])
                {
                    int r = UnityEngine.Random.Range(0,10);
                    if (r >= 1)
                    {
                        ecb.SetComponent(e, new LocalToWorldTransform
                        {
                            Value = ltwt.Value
                        });
                        currentLayer[ix2, iy2] = true;
                    }
                    else {
                        ecb.DestroyEntity(e);
                        currentLayer[ix2, iy2] = false;
                    }
                }
                else {
                    ecb.DestroyEntity(e);
                    currentLayer[ix2, iy2] = false;
                }
                
                if (ix2 % 4 == 0 && ix2 != 0) { iy2++; ix2 = 0; }
                else ix2++;

                if (iy2 == 5) { break; } // <- not nessecary, but we had it anyway to solve the x7 memory massacre
            }
            else {
                ecb.SetComponent(e, new LocalToWorldTransform
                {
                    Value = ltwt.Value
                });

                currentLayer[ix, iy] = true;
            }
            
            
            if (ix % 4 == 0 &&ix!=0) { iy++; ix = 0; }
            else ix++;
            
            
        }

        state.Enabled = false;
    }


}