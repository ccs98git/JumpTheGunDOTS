using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;


public class RayCaster : MonoBehaviour
{
    [SerializeField]
    Vector2 mousePos;
    Vector2 normalVec;
    Vector2 center = new Vector2(960, 540);
    Vector2 mouseVector;
    float finalAngle;

    private Entity en;
    private World wrld;


    private void Start()
    {
        Vector2 v = new Vector2(960, 541);
        normalVec = center - v;
        wrld = World.DefaultGameObjectInjectionWorld;
    }
    void Update()
    {
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (mousePos.x > 960)
        {
            //Right
            mouseVector = center - mousePos;
            finalAngle = Vector2.Angle(mouseVector, normalVec);
        }
        else
        {
            // left
            mouseVector = mousePos - center;
            finalAngle = Vector2.Angle(mouseVector, normalVec) + 180.0f;
        }


        DirectionByAngle(finalAngle);
        /*
        if (wrld.IsCreated && !wrld.EntityManager.Exists(en)) {
            wrld.EntityManager
                .GetBuffer<MouseAngleInput>(en)
                .Add(new MouseAngleInput() { direction = DirectionByAngle(finalAngle) });
        }
        */

    }

    // Returns the direction for the ball to go as an int.
    private int DirectionByAngle(float ang) {
        if ((ang > 0 && ang < 22.5f) || (ang > 337.5f))
        {
            Debug.Log("UU");
            return 0;
        }
        else if (ang < 67.5f)
        {
            Debug.Log("UR");
            return 1;
        }
        else if (ang < 112.5f)
        {
            Debug.Log("RR");
            return 2;
        }
        else if (ang < 157.5f)
        {
            Debug.Log("RD");
            return 3;
        }
        else if (ang < 202.5f)
        {
            Debug.Log("DD");
            return 4;
        }
        else if (ang < 247.5f)
        {
            Debug.Log("DL");
            return 5;
        }
        else if (ang < 292.5f)
        {
            Debug.Log("LL");
            return 6;
        }
        else if (ang < 337.5f)
        {
            Debug.Log("LU");
            return 7;
        }
        else {

            Debug.Log("ERROR");
            return 0;
        }
    }

    public struct MouseAngleInput : IBufferElementData
    {
        public int direction;
    }
}
