using Unity.Entities;
using Unity.Mathematics;

// Ball Tag
struct Ball : IComponentData
{
    // Origin Tile
    public int xGrid;
    public int yGrid;
    public float height;

    // Destination Tile
    public int xGridGoal;
    public int yGridGoal;

    // Direction ball is headed represented in ints.
    public int currentDirection;

    // Parabola Ref
    public Parabola par;
    public float parabolaTimePosition;

    // ?
    public float3 pos;

}

