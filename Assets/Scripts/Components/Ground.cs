using Unity.Entities;

// Ground object
struct Ground : IComponentData
{
    // - Elevation / height
    public int height;
    public bool hasCannon;
    //public (int, int) xy; // <- integer based grid positions.
    public int xPos;
    public int yPos;
}
