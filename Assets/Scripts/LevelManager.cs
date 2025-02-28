// TODO: remove this later if not required
// Uncomment the following line to enable level manager logs
//#define LEVEL_MANAGER_LOGGING

using System;
using UnityEngine;

/// <summary>
/// Class that handles the game flow within a given level
/// </summary>
public class LevelManager : MonoBehaviour
{
    private const string DEFAULT_LEVEL_NAME = "testLevel7x7";
    public string levelToLoad;
    private void Awake()
    {
        GameEvents.InputDetectedEvent -= OnInputDetected;
        GameEvents.InputDetectedEvent += OnInputDetected;
    }

    private void Start()
    {
        //TODO: get the level file name from another manager
        string levelName = String.IsNullOrEmpty(levelToLoad) ? DEFAULT_LEVEL_NAME : levelToLoad;
        LevelData levelData = LevelJSONReader.ReadJSON(levelName, DEFAULT_LEVEL_NAME);
        
        //TODO: validate the level data if possible before broadcasting it
        GameEvents.RaiseLevelDataReadyEvent(levelData);
    }

    private void OnDestroy()
    {
        GameEvents.InputDetectedEvent -= OnInputDetected;
    }

    private void OnInputDetected(Vector3 inputWorldPosition)
    {
#if LEVEL_MANAGER_LOGGING
        Debug.Log($"input detected, world position: {inputWorldPosition}");
#endif
    }
}
