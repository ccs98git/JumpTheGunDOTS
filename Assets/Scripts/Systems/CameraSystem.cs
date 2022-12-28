using Unity.Entities;
using Unity.Transforms;

// - Taken from the DOTS examples
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