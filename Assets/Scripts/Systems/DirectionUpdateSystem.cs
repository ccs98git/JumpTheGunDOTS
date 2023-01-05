using Unity.Burst;
using Unity.Entities;

[BurstCompile]
partial struct DirectionUpdateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    { }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {

        // this system only updates the direction so we can burst the ball movement
        var config = SystemAPI.GetSingletonRW<Config>();
        config.ValueRW.direction = RayCaster.Instance.dirInt;
    }
}
