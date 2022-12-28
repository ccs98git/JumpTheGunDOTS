using Unity.Entities;
using Unity.Mathematics;

public struct Parabola : IComponentData
{
    public float2 start;
    public float2 end;
    public float duration;
    public float a;
    public float b;
    public float c;
}
