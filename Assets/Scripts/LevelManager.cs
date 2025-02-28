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
    private const string DEFAULT_LEVEL_NAME = "testLevel7x7"; // default level that is used as fallback in case we do not find level to load
    
    public string levelToLoad; // name of the level that we want to load

    private int moveCount; // number of moves left in the level
    private void Awake()
    {
        GameEvents.InputDetectedEvent -= OnInputDetected;
        GameEvents.InputDetectedEvent += OnInputDetected;

        GameEvents.MoveCompletedEvent -= OnMoveCompleted;
        GameEvents.MoveCompletedEvent += OnMoveCompleted;
    }

    private void Start()
    {
        //TODO: get the level file name from another manager
        string levelName = String.IsNullOrEmpty(levelToLoad) ? DEFAULT_LEVEL_NAME : levelToLoad;
        LevelData levelData = LevelJSONReader.ReadJSON(levelName, DEFAULT_LEVEL_NAME);
        
        //TODO: validate the level data if possible before broadcasting it

        moveCount = levelData.startingMoveCount;
        
        GameEvents.RaiseLevelDataReadyEvent(levelData);
    }

    private void OnDestroy()
    {
        GameEvents.InputDetectedEvent -= OnInputDetected;
        GameEvents.MoveCompletedEvent += OnMoveCompleted;
    }

    private void OnInputDetected(Vector3 inputWorldPosition)
    {
#if LEVEL_MANAGER_LOGGING
        Debug.Log($"input detected, world position: {inputWorldPosition}");
#endif
    }

    private void OnMoveCompleted()
    {

    }
}
