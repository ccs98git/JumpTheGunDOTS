using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
[BurstCompile]
public static class ParabolaSolve
{
    [BurstCompile]
    public static Parabola Create(float startY, float height, float endY)
    {
        Parabola par = new Parabola();

        par.c = startY;

        float k = math.sqrt(math.abs(startY - height)) / (math.sqrt(math.abs(startY - height)) + math.sqrt(math.abs(endY - height)));
        par.a = (height - startY - k * (endY - startY)) / (k * k - k);

        par.b = endY - startY - par.a;

        return par;
    }

    [BurstCompile]
    public static float Solve(Parabola data, float t)
    {
        return data.a * t * t + data.b * t + data.c;
    }
}

