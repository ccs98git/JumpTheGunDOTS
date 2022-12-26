using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

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
        SimpleCameraFollow.Instance.UpdateTargetPosition(translation.Value);
    }
}