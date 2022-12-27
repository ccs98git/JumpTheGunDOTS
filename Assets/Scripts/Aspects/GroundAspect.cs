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
    //public (int, int) pos => m_ground.ValueRO.xy;
    public int xPos => m_ground.ValueRW.xPos;
    public int yPos => m_ground.ValueRW.yPos;   
    
    public int color => m_ground.ValueRW.color;


    public void FlagCannon(bool b) {
        m_ground.ValueRW.hasCannon = b;
    }

    // For decrements of red color on the ground.
    // Needs to be updated elsewhere. (Ground System?)
    public void DecrementColor() {
        m_ground.ValueRW.color -= 8;
    }
}