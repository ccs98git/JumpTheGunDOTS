using Unity.Entities;
using Unity.Mathematics;

public static class Parabola
{
    public static float3 Create(float startY, float height, float endY)
    {
        float a, b, c;
        c = startY;

        float k = math.sqrt(math.abs(startY - height)) / (math.sqrt(math.abs(startY - height)) + math.sqrt(math.abs(endY - height)));
        a = (height - startY - k * (endY - startY)) / (k * k - k);

        b = endY - startY - a;
        return new float3(a, b, c);
    }
    public static float Solve(float a, float b, float c, float t)
    {
        return a * t * t + b * t + c;
    }
}
