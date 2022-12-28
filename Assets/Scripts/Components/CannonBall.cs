using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Rendering;

public struct CannonBall : IComponentData
{
    public Parabola par;
    public Life lifeLived;
}
