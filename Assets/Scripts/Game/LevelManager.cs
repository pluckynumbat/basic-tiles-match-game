// Uncomment the following line to enable level manager logs
//#define LEVEL_MANAGER_LOGGING

using UnityEngine;

/// <summary>
/// Class that handles the game state changes within a given level
/// </summary>
public class LevelManager : MonoBehaviour
{
    private int moveCount; // number of moves left in the level
    private int incompleteGoalsLeft; // number of unfulfilled goals left in the level
    private void Awake()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.LevelDataReadyEvent += OnLevelDataReady;
        
        GameEvents.GoalCompletedEvent -= OnGoalCompleted;
        GameEvents.GoalCompletedEvent += OnGoalCompleted;
        
        GameEvents.MoveCompletedEvent -= OnMoveCompleted;
        GameEvents.MoveCompletedEvent += OnMoveCompleted;
    }

    private void Start()
    {
        // request level data
        GameEvents.RaiseLevelDataRequestEvent();
    }

    private void OnDestroy()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.GoalCompletedEvent -= OnGoalCompleted;
        GameEvents.MoveCompletedEvent -= OnMoveCompleted;
    }

    // store starting move count and goal count
    private void OnLevelDataReady(LevelData levelData)
    {
        moveCount = levelData.startingMoveCount;
        incompleteGoalsLeft = levelData.goals.Count;
    }

    //check incomplete goal count
    private void OnGoalCompleted(LevelGoal.GoalType goalType)
    {
        if (moveCount <= 0) // do not mark level goal completion once a level is already over
        {
            return;
        }
        
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

        if (moveCount <= 0 && incompleteGoalsLeft > 0)
        {
#if LEVEL_MANAGER_LOGGING
            Debug.Log("Level ended, you lost :( ");
#endif
            // let other systems know that the level ended, and that the player lost
            GameEvents.RaiseLevelEndedEvent(false);
        }
    }
}
