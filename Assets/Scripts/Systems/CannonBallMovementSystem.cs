using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public partial struct CannonBallMovementSystem : ISystem
{


    public void OnCreate(ref SystemState state)
    {
        

    }

    public void OnDestroy(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {

        foreach (var (timerPar, parabolaData, translation) in SystemAPI.Query<RefRW<TimePar>,
                     RefRW<Parabola>, TransformAspect>())
        {
            var localTime = timerPar.ValueRW.parTime + SystemAPI.Time.DeltaTime / parabolaData.ValueRO.duration;
            var newTrans = new float3(
                math.lerp(parabolaData.ValueRO.start.x, parabolaData.ValueRO.end.x, localTime),
                ParabolaSolve.Solve(parabolaData.ValueRW, localTime),
                math.lerp(parabolaData.ValueRW.start.y, parabolaData.ValueRW.end.y, localTime));

            translation.Position = newTrans;
            timerPar.ValueRW.parTime = localTime;

            
        }





    }
}
public partial struct DestroyCannonBalls : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnDestroy(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (transPos, par, timerLife, entity) in SystemAPI.Query<CannonBall, RefRO<Parabola>, RefRO<TimePar>>().WithAll<CannonBall>().WithEntityAccess())
        {
            if (timerLife.ValueRO.parTime >= 1.0f)
            {
                // Looks up ground tile and lowers it if it is above the minimum height
                // Rounds to integer values -> meaning it rounds to whole numbers for ground identification
                // through rough approximation.
                (int, int) CB_pos = ((int)math.round(par.ValueRO.end.x), (int)math.round(par.ValueRO.end.y)); 

                foreach (var (groundElement, groundEntity) in SystemAPI.Query<Ground>().WithEntityAccess()) {
                    if (groundElement.xPos == CB_pos.Item1 && groundElement.yPos == CB_pos.Item2) {
                        // When the tile is found -> check height index. If above 1
                        // Lower by 1 and relocate the object so it reduces its height by 1
                        if (groundElement.height > 1)
                        {

                            int newheight = groundElement.height - 1;
                            int newColorForGround = groundElement.color + 8;
                            // update color of ground
                            Vector3 flatColor = new Vector3(newColorForGround, 202f, 56f).normalized; //<- not sure if/why this works, or if it works as intended.
                            Color newColor = UnityEngine.Color.HSVToRGB(flatColor.x, flatColor.y, flatColor.z);

                            ecb.SetComponent(groundEntity, new URPMaterialPropertyBaseColor
                            {
                                Value = (UnityEngine.Vector4)newColor
                            });

                            // Relocate and resize the ground element.
                            float heightIndex = 0.2f * (newheight); // <- random int between 1 and maxHeight (inclusive)

                            // Transform Manipulation, place and scale.
                            float3 Position = new float3(groundElement.xPos, (heightIndex / 2.0f), groundElement.yPos);
                            float3 newScale = new float3(1, 1 + heightIndex, 1);

                            ecb.SetComponent(groundEntity, new Translation
                            {
                                Value = Position
                            });
                            ecb.AddComponent(groundEntity, new NonUniformScale
                            {
                                Value = newScale
                            });

                            
                            // update last - shadow updating causes some funky buisness going on.
                            
                            ecb.SetComponent(groundEntity, new Ground
                            {
                                height = newheight,
                                color = newColorForGround,
                                xPos = groundElement.xPos,
                                yPos = groundElement.yPos,
                                hasCannon = groundElement.hasCannon
                            });

                            
                            
                            break; // <- since each cannonball only has one desitnation, we can break
                            // the loop and carry on at this point.
                        }
                        else {
                            break; // found tile, no edits needed. Break loop since same logic as above.
                        }

                    }
                }

                // Destroys Cannonball Entity - an evnetuality
                ecb.DestroyEntity(entity);
            }
        }
        

    }
}
