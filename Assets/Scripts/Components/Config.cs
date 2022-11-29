using Unity.Entities;

// Config object
struct Config : IComponentData
{
    public Entity Ground;
    public Entity Ball;
    public Entity Cannon;

    public int xScale;
    public int yScale;
}

