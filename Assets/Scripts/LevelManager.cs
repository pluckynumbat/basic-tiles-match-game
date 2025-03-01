// TODO: remove this later if not required
// Uncomment the following line to enable level manager logs
#define LEVEL_MANAGER_LOGGING

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
        GameEvents.GoalCompletedEvent -= OnGoalCompleted;
        GameEvents.GoalCompletedEvent += OnGoalCompleted;
        
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
        incompleteGoalsLeft = levelData.goals.Count;
        
        GameEvents.RaiseLevelDataReadyEvent(levelData);
    }

    private void OnDestroy()
    {
        GameEvents.GoalCompletedEvent -= OnGoalCompleted;
        GameEvents.MoveCompletedEvent -= OnMoveCompleted;
    }
    
    //check incomplete goal count
    private void OnGoalCompleted(LevelGoal.GoalType goalType)
    {
        incompleteGoalsLeft  -= 1; // reduce incomplete goals left by 1

#if LEVEL_MANAGER_LOGGING
        Debug.Log($"Incomplete goals count: {incompleteGoalsLeft}");
#endif
        if (incompleteGoalsLeft <= 0)
        {
#if LEVEL_MANAGER_LOGGING
            Debug.Log("Level ended, You Won!");
#endif
            // let other systems know that the level ended, and that the player won
            GameEvents.RaiseLevelEndedEvent(true);           
        }
    }

    //update move count and check if the level ends
    private void OnMoveCompleted()
    {
        if (moveCount <= 0)
        {
            return;
        }

        moveCount -= 1; // reduce remaining move count by 1
#if LEVEL_MANAGER_LOGGING
        Debug.Log($"Move count: {moveCount}");
#endif

        if (moveCount <= 0)
        {
#if LEVEL_MANAGER_LOGGING
            Debug.Log("Level ended, you lost :( ");
#endif
            // let other systems know that the level ended, and that the player lost
            GameEvents.RaiseLevelEndedEvent(false);
        }
    }
}
