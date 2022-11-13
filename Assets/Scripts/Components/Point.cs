using Unity.Entities;

// Component: point object
struct Point : IComponentData
{
    public float x;
    public float y;
    public float z;

    public float oldX;
    public float oldY;
    public float oldZ;

    public bool anchor;

    public int neighborCount;
}
