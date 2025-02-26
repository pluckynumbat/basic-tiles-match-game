using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the game flow within a given level
/// </summary>
public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        GameEvents.InputDetectedEvent -= OnInputDetected;
        GameEvents.InputDetectedEvent += OnInputDetected;
    }
    
    private void OnDestroy()
    {
        GameEvents.InputDetectedEvent -= OnInputDetected;
    }

    private void OnInputDetected(Vector3 inputWorldPosition)
    {
        Debug.Log($"input detected, world position: {inputWorldPosition}");
    }
}
