using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleCameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public float xRot;
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
        mainCamera.transform.localRotation.eulerAngles.Set(xRot, 0, 0);
    }

    public void UpdateTargetPosition(float3 position)
    {
        targetPosition = position;
    }
}
