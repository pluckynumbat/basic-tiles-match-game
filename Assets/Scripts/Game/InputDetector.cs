using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic script used to detect input from the player
/// </summary>
public class InputDetector : MonoBehaviour
{
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            GameEvents.RaiseInputDetectedEvent(mouseWorldPosition);
        }
    }
}
