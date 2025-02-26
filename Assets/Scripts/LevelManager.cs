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

    private void Start()
    {
        //TODO: get the level file name from another manager
        LevelData levelData = LevelJSONReader.ReadJSON("testLevel1");
        
        //TODO: validate the level data if possible before broadcasting it
        GameEvents.RaiseLevelDataReadyEvent(levelData);
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
