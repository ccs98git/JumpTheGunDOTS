using Unity.Entities;

// Ball Tag
struct Cannon : IComponentData
{
    public Entity CannonBall;
    public Entity SpawnBallPoint;
    public float coolDown;
}