// TODO: remove this later if not required
// Uncomment the following line to enable level goals logs
#define LEVEL_GOALS_LOGGING

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
        
        GameEvents.GridCellsCollectedEvent -= OnGridCellsCollected;
        GameEvents.GridCellsCollectedEvent += OnGridCellsCollected;
    }

    private void OnDestroy()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.GridCellsCollectedEvent -= OnGridCellsCollected;
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
        
        // Debug Only TODO: remove this later?
        PrintGoalStatusToConsole();
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
    
    // update the goals dictionary, see if any new goal was completed, see if all goals were completed
    private void OnGridCellsCollected(List<GameGridCell> gridCellsCollected)
    {
        bool progressUpdated = false;
        
        // Part 1. all the grid cells should be of the same color, so just check the color of the first one
        GameGridCell.GridCellColor color = gridCellsCollected[0].Color;
        
        //get goal type from the color
        LevelGoal.GoalType goalType = GetGoalTypeFromGridCellColor(color);
        
        // check if this color has a valid goal type, that it is a goal for this level, and that goal has not been completed yet
        if (goalType != LevelGoal.GoalType.None && goalProgress.ContainsKey(goalType) && goalProgress[goalType].Remaining > 0)
        {
            UpdateGoalProgress(goalType, gridCellsCollected.Count);
            progressUpdated = true;
        }
        
        // Part 2. also check if there is a collect any goal type that is incomplete, since these will count towards that as well
        if (goalProgress.ContainsKey(LevelGoal.GoalType.CollectAny) && goalProgress[LevelGoal.GoalType.CollectAny].Remaining > 0)
        {
            UpdateGoalProgress(LevelGoal.GoalType.CollectAny, gridCellsCollected.Count);
            progressUpdated = true;
        }
        
        // Part 3. finally, in case progress was updated,check if all goals are completed,
        // in which case the level is over and the player won!
        if (progressUpdated)
        {
            bool foundIncompleteGoal = false;
            foreach (LevelGoal goal in goalProgress.Values)
            {
                if (goal.Remaining > 0)
                {
                    foundIncompleteGoal = true;
                    break;
                }
            }

            if (!foundIncompleteGoal)
            { 
#if LEVEL_GOALS_LOGGING
                Debug.Log("Level ended! You Won!");
#endif              
                GameEvents.RaiseLevelEndedEvent(true);
            }
        }
        
        // Debug Only TODO: remove this later?
        PrintGoalStatusToConsole();
    }
    
    // function to update goal progress for a given goal, and raise the goal completed event if applicable
    private void UpdateGoalProgress(LevelGoal.GoalType goalType, int amount)
    {
        //update progress!
        goalProgress[goalType].Remaining -= amount;
            
        //if it is newly completed, let other systems know
        if (goalProgress[goalType].Remaining <= 0)
        {
            goalProgress[goalType].Remaining = 0; // lower limit is 0
#if LEVEL_GOALS_LOGGING
            Debug.Log($"Level goal completed: {goalType}!");
#endif      
            GameEvents.RaiseGoalCompletedEvent(goalType);
        }
    }


    // helper to get goal type from a Grid cell color type
    private LevelGoal.GoalType GetGoalTypeFromGridCellColor(GameGridCell.GridCellColor color)
    {
        switch (color)
        {
            case GameGridCell.GridCellColor.Red:
                return LevelGoal.GoalType.CollectRed;
            
            case GameGridCell.GridCellColor.Green:
                return LevelGoal.GoalType.CollectGreen;
            
            case GameGridCell.GridCellColor.Blue:
                return LevelGoal.GoalType.CollectBlue;
            
            case GameGridCell.GridCellColor.Yellow:
                return LevelGoal.GoalType.CollectYellow;
        }
        
        Debug.LogError($"Goal type does not exist for goal color: {color}");
        return LevelGoal.GoalType.None;
    }

    //debug only helper to print goal status for the level
    private void PrintGoalStatusToConsole()
    {
        string goalStatusString = "Overall Goal Status: \n";
        foreach (LevelGoal goal in goalProgress.Values)
        {
            goalStatusString += $"Goal: {goal.Type}, Total: {goal.TotalAmount}, Remaining: {goal.Remaining} \n";
        }
#if LEVEL_GOALS_LOGGING
        Debug.Log(goalStatusString);
#endif
    }
}