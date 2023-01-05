using UnityEngine;
using Unity.Entities;

// Ground object
struct Ground : IComponentData
{
    // - Elevation / height
    public int height;
    public bool hasCannon;
    
    public int xPos;
    public int yPos;

    public int color;
}
