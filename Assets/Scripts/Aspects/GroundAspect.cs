using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

readonly partial struct GroundAspect : IAspect
{
    readonly RefRW<Ground> m_ground;
    public int height => m_ground.ValueRW.height;
    public bool hasCannon => m_ground.ValueRW.hasCannon;
    public int xPos => m_ground.ValueRW.xPos;
    public int yPos => m_ground.ValueRW.yPos;   
    
    public int color => m_ground.ValueRW.color;


    public void FlagCannon(bool b) {
        m_ground.ValueRW.hasCannon = b;
    }

}
