using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

readonly partial struct CannonAspect : IAspect
{
    readonly RefRW<Cannon> m_cannon;
    public readonly TransformAspect transform;
    public Entity CannonBallSpawn => m_cannon.ValueRO.SpawnBallPoint;
    public Entity CannonBallPrefab => m_cannon.ValueRO.CannonBall;
    
}