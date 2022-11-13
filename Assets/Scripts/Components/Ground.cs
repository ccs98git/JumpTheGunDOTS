using Unity.Entities;

// Ground object
struct Ground : IComponentData
{
    // - Elevation / height
    public int height;
    public UnityEngine.Color clr;

}
