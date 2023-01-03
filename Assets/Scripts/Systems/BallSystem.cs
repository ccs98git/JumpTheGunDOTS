using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using JetBrains.Annotations;
using static Unity.Burst.Intrinsics.X86.Avx;
using System.Net;
using System.Runtime.InteropServices;

[BurstCompile]
partial struct BallSystem : ISystem
{
    

    // all ISystem derived methods need to be implemented, even if empty.
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    { }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
    [BurstCompile]
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
		var config = SystemAPI.GetSingleton<Config>();

        if (config.setupStage >= 3) {
            // Ball movement ---
            var player_ball = SystemAPI.GetSingletonRW<Ball>();
            var player_entity = SystemAPI.GetSingletonEntity<Ball>();
            var player = SystemAPI.GetAspectRW<BallAspect>(player_entity);

                // Write position (clamped) data to the ball.
                // Needs to be rounded, as integer casting trunctuates it, making negative directions count twice for some reason.
                player_ball.ValueRW.xGrid = (int)math.round(player.transform.Position.x);
                player_ball.ValueRW.yGrid = (int)math.round(player.transform.Position.z);
                player_ball.ValueRW.height = player.transform.Position.y;

                // Read Data from Ball
                int xGridCurrent = player_ball.ValueRO.xGrid;
                int yGridCurrent = player_ball.ValueRO.yGrid;

                // if false -> open up for new goal to be set.
                bool isTraversing = player_ball.ValueRO.isTraversing;

                // Read the direction from RayCaster
                //player_ball.ValueRW.currentDirection = RayCaster.Instance.dirInt;

                // Check to see that the ball is at it's destination. If it is - then make a new goal.
                // If it is not - Then schedule traversal job.
                //if (xGridCurrent == xGridGoal && yGridCurrent == yGridGoal) {
                //    isTraversing = false;
                //}

                if (!isTraversing)
                {
                    player_ball.ValueRW.currentDirection = RayCaster.Instance.dirInt;
                    //AssignDirectionToBall(player_ball);
                    
                    // Determine a new goal to the ball based on direction
                    if (player_ball.ValueRO.currentDirection == 0)
                    {
                        player_ball.ValueRW.yGridGoal = yGridCurrent + 1;
                    }
                    else if (player_ball.ValueRO.currentDirection == 1)
                    {
                        player_ball.ValueRW.xGridGoal = xGridCurrent + 1;
                        player_ball.ValueRW.yGridGoal = yGridCurrent + 1;
                    }
                    else if (player_ball.ValueRO.currentDirection == 2)
                    {
                        player_ball.ValueRW.xGridGoal = xGridCurrent + 1;
                    }
                    else if (player_ball.ValueRO.currentDirection == 3)
                    {
                        player_ball.ValueRW.xGridGoal = xGridCurrent + 1;
                        player_ball.ValueRW.yGridGoal = yGridCurrent - 1;
                    }
                    else if (player_ball.ValueRO.currentDirection == 4)
                    {
                        player_ball.ValueRW.yGridGoal = yGridCurrent - 1;
                    }
                    else if (player_ball.ValueRO.currentDirection == 5)
                    {
                        player_ball.ValueRW.xGridGoal = xGridCurrent - 1;
                        player_ball.ValueRW.yGridGoal = yGridCurrent - 1;
                    }
                    else if (player_ball.ValueRO.currentDirection == 6)
                    {
                        player_ball.ValueRW.xGridGoal = xGridCurrent - 1;
                    }
                    else if (player_ball.ValueRO.currentDirection == 7)
                    {
                        player_ball.ValueRW.xGridGoal = xGridCurrent - 1;
                        player_ball.ValueRW.yGridGoal = yGridCurrent + 1;
                    }

                // Checks if the destination is within game limits. if not -> Stall ball
                if (player_ball.ValueRO.xGridGoal >= 0 && player_ball.ValueRO.xGridGoal < config.xScale
                        && player_ball.ValueRO.yGridGoal >= 0 && player_ball.ValueRO.yGridGoal < config.yScale)
                {

                    // Query Ground for goal tile
                    foreach (var groundA in SystemAPI.Query<GroundAspect>())
                    {
                        if (groundA.xPos == player_ball.ValueRO.xGridGoal && groundA.yPos == player_ball.ValueRO.yGridGoal)
                        {
                            if (!groundA.hasCannon)
                            {
                                // If the ground does not have a cannon, create parabola and resolve as normal
                                player_ball.ValueRW.par = ParabolaSolve.Create(player_ball.ValueRO.height, ((groundA.height * 0.2f) + 2), groundA.height * 0.2f);
                                player_ball.ValueRW.par.start = new float2(player_ball.ValueRO.xGrid, player_ball.ValueRO.yGrid);
                                player_ball.ValueRW.par.end = new float2(groundA.xPos, groundA.yPos);
                                player_ball.ValueRW.par.duration = 1.5f;
                            } 
                            else {
                                // If the ground has a cannon, stall till propper input is given.
                                StallBall(player_ball);
                            }
                            break;
                        }
                    }
                }
                else {
                    StallBall(player_ball);
                }

                    player_ball.ValueRW.isTraversing = true;
                }

                // --- If the ball is traversing - calculate position based on time. --- 

                else {
                    var parabolaData = player_ball.ValueRW.par;
                    player_ball.ValueRW.parabolaTimePosition += SystemAPI.Time.DeltaTime / parabolaData.duration;
                    

                    var newTrans = new float3(
                        math.lerp(parabolaData.start.x, parabolaData.end.x, player_ball.ValueRW.parabolaTimePosition),
                        ParabolaSolve.Solve(parabolaData, player_ball.ValueRW.parabolaTimePosition),
                        math.lerp(parabolaData.start.y, parabolaData.end.y, player_ball.ValueRW.parabolaTimePosition));

                    if (player_ball.ValueRW.parabolaTimePosition >= 1)
                    {
                        player_ball.ValueRW.xGrid = player_ball.ValueRO.xGridGoal;
                        player_ball.ValueRW.yGrid = player_ball.ValueRO.yGridGoal;

                        player_ball.ValueRW.parabolaTimePosition = 0;
                        player_ball.ValueRW.isTraversing = false;

                }

                player.transform.LocalPosition = newTrans;

                //Schedule a job to move the ball towards it's current direction.
                // Doesn't work because DOTS
                /*
                new UpdateBallPosJob
                {
                    t = SystemAPI.Time.DeltaTime
                }.Run();
                */

            }
        }

    }

    // Stalls the ball by creating parabolas with the end centered on the start
    // And resetting goal variables so they do not run away
    [BurstCompile]
    private void StallBall(RefRW<Ball> b) {
        // bounce in place untill a valid direction is given.
        b.ValueRW.par = ParabolaSolve.Create(b.ValueRO.height, (3), b.ValueRO.height);
        b.ValueRW.par.start = new float2(b.ValueRO.xGrid, b.ValueRO.yGrid);
        b.ValueRW.par.end = new float2(b.ValueRO.xGrid, b.ValueRO.yGrid);
        b.ValueRW.par.duration = 1.5f;
        // - Make sure the goal numbers don't just build up in the background for every bad jump
        b.ValueRW.xGridGoal = b.ValueRO.xGrid;
        b.ValueRW.yGridGoal = b.ValueRO.yGrid;
    }

    //private void AssignDirectionToBall(RefRW<Ball> b) { b.ValueRW.currentDirection = RayCaster.Instance.dirInt; }
    /*
    [BurstCompile]
    private void SetBallPosition(RefRW<Ball> b, int x, int y) { 
        b.ValueRW.xGrid = x;
        b.ValueRW.yGrid = y;
    }
    */

    /*
    [WithAll(typeof(Ball))]
    public partial struct UpdateBallPosJob : IJobEntity {
        //public float t;
        public void Execute(ref Translation translation, ref Ball b) {
            //float3 dir = new float3(0,0,0);
            //int dir ection = b.currentDirection;
            

            var parabolaData = b.par;
            //var localTime = SystemAPI.Time.DeltaTime / parabolaData.duration;
            b.parabolaTimePosition += t / parabolaData.duration;
            if (b.parabolaTimePosition >= 1) { 
                b.parabolaTimePosition = 0;
            }

            var newTrans = new float3(
                math.lerp(parabolaData.start.x, parabolaData.end.x, b.parabolaTimePosition),
                ParabolaSolve.Solve(parabolaData, b.parabolaTimePosition),
                math.lerp(parabolaData.start.y, parabolaData.end.y, b.parabolaTimePosition));
            
            
            translation.Value = newTrans;
            
            
            /*
            if (direction == 0)
            {
                dir = new float3(0, 0, 0.01f);
            }
            else if (direction == 1)
            {
                dir = new float3(0.01f, 0, 0.01f);
            }
            else if (direction == 2)
            {
                dir = new float3(0.01f, 0, 0.0f);
            }
            else if (direction == 3)
            {
                dir = new float3(0.01f, 0, -0.01f);
            }
            else if (direction == 4)
            {
                dir = new float3(0.0f, 0, -0.01f);
            }
            else if (direction == 5)
            {
                dir = new float3(-0.01f, 0, -0.01f);
            }
            else if (direction == 6)
            {
                dir = new float3(-0.01f, 0, 0);
            }
            else if (direction == 7)
            {
                dir = new float3(-0.01f, 0, 0.01f);
            }
            


        }
    
    }
    */
    /*
     public partial class CameraSystem : SystemBase
{
    protected override void OnUpdate()
    {
        new UpdateCameraJob().Run();
    }
}

[WithAll(typeof(Ball))]
public partial struct UpdateCameraJob : IJobEntity
{
    public void Execute(ref Translation translation)
    {
        CameraOperator.Instance.UpdateTargetPosition(translation.Value);
    }
}
     */

    /* // Traversal in Job
                if (RayCaster.Instance.dirInt == 0)
                    {
                        var dir = new float3(0, 0, 0.01f);
                        player.transform.LocalPosition += dir;

                    }
                    else if (RayCaster.Instance.dirInt == 1)
                    {
                        var dir = new float3(0.005f, 0, 0.005f);
                        player.transform.LocalPosition += dir;
                    }
                    else if (RayCaster.Instance.dirInt == 2)
                    {
                        var dir = new float3(0.01f, 0, 0.0f);
                        player.transform.LocalPosition += dir;
                    }
                    else if (RayCaster.Instance.dirInt == 3)
                    {
                        var dir = new float3(0.005f, 0, -0.005f);
                        player.transform.LocalPosition += dir;
                    }
                    else if (RayCaster.Instance.dirInt == 4)
                    {
                        var dir = new float3(0.0f, 0, -0.01f);
                        player.transform.LocalPosition += dir;
                    }
                    else if (RayCaster.Instance.dirInt == 5)
                    {
                        var dir = new float3(-0.005f, 0, -0.005f);
                        player.transform.LocalPosition += dir;
                    }
                    else if (RayCaster.Instance.dirInt == 6)
                    {
                        var dir = new float3(-0.01f, 0, 0);
                        player.transform.LocalPosition += dir;
                    }
                    else if (RayCaster.Instance.dirInt == 7)
                    {
                        var dir = new float3(-0.005f, 0, 0.005f);
                        player.transform.LocalPosition += dir;
                    }
     */
}