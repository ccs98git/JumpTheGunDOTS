using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

// - Taken from the DOTS examples
public class SimpleCameraFollow : MonoBehaviour
{
    public Vector3 offset;
    Vector3 targetPosition;
    private Camera mainCamera;

    public static SimpleCameraFollow Instance;

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

    void Start()
    {
        mainCamera = Camera.main;
       
    }

    private void LateUpdate()
    {
        mainCamera.gameObject.transform.position = targetPosition + offset;
    }

    public void UpdateTargetPosition(float3 position)
    {
        targetPosition = position;
    }
}
