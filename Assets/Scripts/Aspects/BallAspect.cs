using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

readonly partial struct BallAspect : IAspect
{
    readonly RefRW<Ball> m_ball;
    public readonly TransformAspect transform;
}

