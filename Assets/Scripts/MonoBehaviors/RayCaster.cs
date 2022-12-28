using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


public class RayCaster : MonoBehaviour
{

    [SerializeField]
    Vector2 mousePos;
    Vector2 normalVec;
    Vector2 center = new Vector2(960, 540);
    Vector2 mouseVector;
    float finalAngle;

    private World wrld;

    public int dirInt = -1;
    public static RayCaster Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

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


        //DirectionByAngle(finalAngle);

        EntityQuery q = wrld.EntityManager.CreateEntityQuery(new EntityQueryDesc()
        {
            All = new ComponentType[] { ComponentType.ReadWrite<Config>() }
        });

        if (q.TryGetSingleton<Config>(out var singleton))
        {
            // Singleton Component Approach
            //singleton.direction += DirectionByAngle(finalAngle);

            /* // Entity Approach
            wrld.EntityManager.SetComponentData<Config>(singleton, new Config { 
                direction = DirectionByAngle(finalAngle)
            });
            */

            //Debug.Log(singleton.direction);
        }

        var ent = q.GetSingletonEntity();

        dirInt = DirectionByAngle(finalAngle);
    }

    // Returns the direction for the ball to go as an int.
    private int DirectionByAngle(float ang) {
        if ((ang > 0 && ang < 22.5f) || (ang > 337.5f))
        {
            return 0;
        }
        else if (ang < 67.5f)
        {
            return 1;
        }
        else if (ang < 112.5f)
        {
            return 2;
        }
        else if (ang < 157.5f)
        {
            return 3;
        }
        else if (ang < 202.5f)
        {
            return 4;
        }
        else if (ang < 247.5f)
        {
            return 5;
        }
        else if (ang < 292.5f)
        {
            return 6;
        }
        else if (ang < 337.5f)
        {
            return 7;
        }
        else {

            Debug.Log("ERROR");
            return -1;
        }
    }
}
