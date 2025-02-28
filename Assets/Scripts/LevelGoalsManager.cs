using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that tracks and manages all goals related data in the level
/// </summary>
public class LevelGoalsManager : MonoBehaviour
{
    // dictionary that stores all goal progress during a level, keyed by goal type (values are goals)
    private Dictionary<LevelGoal.GoalType, LevelGoal> goalProgress; 
    private void Awake()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.LevelDataReadyEvent += OnLevelDataReady;
    }

    private void OnDestroy()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
    }

    // initialize the goals dictionary
    private void OnLevelDataReady(LevelData levelData)
    {
        goalProgress = new Dictionary<LevelGoal.GoalType, LevelGoal>();
        foreach (LevelGoalData goalData in levelData.goals)
        {
            LevelGoal.GoalType key = GetGoalTypeFromString(goalData.goalType);
            goalProgress[key] = new LevelGoal(key, goalData.goalAmount);
        }
    }

    //helper to get goal type from a string in the level data
    private LevelGoal.GoalType GetGoalTypeFromString(string goalString)
    {
        switch (goalString)
        {
            case "R":
                return LevelGoal.GoalType.CollectRed;
            
            case "G":
                return LevelGoal.GoalType.CollectGreen;
            
            case "B":
                return LevelGoal.GoalType.CollectBlue;
            
            case "Y":
                return LevelGoal.GoalType.CollectYellow;
            
            case "A":
                return LevelGoal.GoalType.CollectAny;
        }
        
        Debug.LogError($"Invalid goal string: {goalString}");
        return LevelGoal.GoalType.None;
    }
}