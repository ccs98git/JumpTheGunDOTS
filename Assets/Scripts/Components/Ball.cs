using Unity.Entities;

// Ball Tag
struct Ball : IComponentData
{
    // Origin Tile
    public int xGrid;
    public int yGrid;

    // Destination Tile
    public int xGridGoal;
    public int yGridGoal;

    // 
    public int currentDirection;

}

