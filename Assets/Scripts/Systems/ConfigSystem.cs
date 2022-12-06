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
        var config = SystemAPI.GetSingletonRW<Config>();

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        
        // -- Scale x and y, edit in the editor for variable entity counts --
        int xScale = config.ValueRO.xScale;
        int yScale = config.ValueRO.yScale;
        // -----------------------------------------------------
        int groundMemAlloc = xScale * yScale;

        // -- Set setup stage --
        if (config.ValueRW.setupStage == 0)
        {
            
            var terrain = CollectionHelper.CreateNativeArray<Entity>(groundMemAlloc, Allocator.Temp);
            ecb.Instantiate(config.ValueRW.Ground, terrain);


            int ix = 0;
            int iy = 0;
            float heightIndex = 0; // <- random height, multiplied by magnitude 2.0f

            foreach (Entity e in terrain)
            {

                int rand = UnityEngine.Random.Range(1, 10);
                heightIndex = 0.2f * rand; // <- random int between 1 and 9 (inclusive)


                // Transform Manipulation, place and scale.
                float3 Position = new float3(ix, (heightIndex / 2.0f), iy);
                float3 newScale = new float3(1, 1 + heightIndex, 1);

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


                // Set height + pos + bool in the Ground Component
                ecb.SetComponent(e, new Ground
                {
                    height = rand, // <- assignment of height index
                    hasCannon = false,
                    //xy = (ix, iy)
                    xPos = ix,
                    yPos = iy
                });

                if (ix % (xScale - 1) == 0 && ix != 0) { iy++; ix = 0; }
                else ix++;
            }

        }
        else if (config.ValueRW.setupStage == 1)
        {

            // cannon generation --

            int cannonMemAlloc = groundMemAlloc / 5;

            var cannon = CollectionHelper.CreateNativeArray<Entity>(cannonMemAlloc, Allocator.Temp);
            ecb.Instantiate(config.ValueRW.Cannon, cannon);


            foreach (Entity e in cannon)
            {
                int randPosX = UnityEngine.Random.Range(0, xScale);
                int randPosY = UnityEngine.Random.Range(0, yScale);

                foreach (var groundE in SystemAPI.Query<GroundAspect>())
                { // <- query to find ALL ground Entities. Not terribly Random access friendly.
                    
                    
                        if (groundE.xPos == randPosX && groundE.yPos == randPosY)
                        { // <- Check for a position.
                            if (groundE.hasCannon == false) // <- if no cannon, place one there.
                            {
                                // place cannon
                                float3 CannonPosition = new float3(randPosX, (groundE.height), randPosY);
                                ecb.SetComponent(e, new Translation
                                {
                                    Value = CannonPosition
                                });

                                groundE.FlagCannon(true);
                                
                            }

                        }
                    
                }
            }
        }
        else if (config.ValueRW.setupStage == 2) {
            state.Enabled = false;
        }



        config.ValueRW.setupStage++;

    }


}