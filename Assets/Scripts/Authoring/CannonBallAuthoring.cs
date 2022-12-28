using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CannonBallAuthoring : UnityEngine.MonoBehaviour
{
}
class CannonBallBaker: Baker<CannonBallAuthoring>
{
    public override void Bake(CannonBallAuthoring authoring)
    {
        AddComponent<CannonBall>();
        AddComponent(new Parabola
        {
            duration = 00001f
        });
        AddComponent(new Life
        {
            lifeTime = 0
        });
    }
}
