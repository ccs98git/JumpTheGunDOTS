using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;

public static class ParabolaSolve
{
    public static Parabola Create(float startY, float height, float endY)
    {
        Parabola par = new Parabola();
        float a, b, c;

        c = startY;

        float k = math.sqrt(math.abs(startY - height)) / (math.sqrt(math.abs(startY - height)) + math.sqrt(math.abs(endY - height)));
        a = (height - startY - k * (endY - startY)) / (k * k - k);

        b = endY - startY - par.a;
        par.a = a;
        par.b = b;
        par.c = c;

        return par;
    }

    /// <summary>
    /// Solves a parabola (in the form y = a*t*t + b*t + c) for y.
    /// </summary>
    public static float Solve(in Parabola data, float t)
    {
        return data.a * t * t + data.b * t + data.c;
    }
}

