using System;
using UnityEngine;

/// <summary>
/// UI script to display the different goals during a level
/// </summary>
public class UIGoalsDisplay : MonoBehaviour
{
    private const int MAX_GOALS_DISPLAYED = 4; // number of goals the display can support
    public UIGoalItem[] goalItems;
    public Sprite[] goalSpriteOptions; // the different sprite options for the goals

    private int goalCount; // number of distinct goals in the level, the display supports 4
    private void Awake()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.LevelDataReadyEvent += OnLevelDataReady;
        
        GameEvents.GoalProgressUpdatedEvent -= OnGoalProgressUpdated;
        GameEvents.GoalProgressUpdatedEvent += OnGoalProgressUpdated;
        
        GameEvents.GoalCompletedEvent -= OnGoalCompleted;
        GameEvents.GoalCompletedEvent += OnGoalCompleted;
    }

    private void OnDestroy()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.GoalProgressUpdatedEvent -= OnGoalProgressUpdated;
        GameEvents.GoalCompletedEvent -= OnGoalCompleted;
    }
    
    // initialize goal items by providing them sprites and goal amounts 
    private void OnLevelDataReady(LevelData levelData)
    {
        SetupGoalsDisplay(levelData);
    }

    // initialize goal items by providing them sprites and goal amounts
    // if allCompleted is true, mark all goals as completed
    public void SetupGoalsDisplay(LevelData levelData, bool allCompleted = false)
    {
        //disable all goal items
        foreach (UIGoalItem goalItem in goalItems)
        {
            goalItem.gameObject.SetActive(false);
        }
        
        goalCount = levelData.goals.Count;

        // we can only show the first 4 goals
        if (goalCount > MAX_GOALS_DISPLAYED)
        {
            Debug.LogError($"Goal count of the level({goalCount}) is larger than the display count capacity({MAX_GOALS_DISPLAYED})!");
        }

        int counter = 0;
        foreach (LevelGoalData goal in levelData.goals)
        {
            if (counter >= MAX_GOALS_DISPLAYED)
            {
                break;
            }

            LevelGoal.GoalType goalType = LevelGoal.GetGoalTypeFromString(goal.goalType);
            goalItems[counter].gameObject.SetActive(true);
            if (allCompleted)
            {
                goalItems[counter].SetupPreCompletedGoalItem(goalType, GetSpriteBasedOnGoalType(goalType));
            }
            else
            {
                goalItems[counter].SetupGoalItem(goalType, GetSpriteBasedOnGoalType(goalType), goal.goalAmount);
            }
            counter++;
        }

        goalCount = Math.Min(goalCount, MAX_GOALS_DISPLAYED);
    }

    private void OnGoalProgressUpdated(LevelGoal.GoalType goalType, int remaining)
    {
        CheckAndUpdateGoalItems(goalType, remaining);
    }
    
    private void OnGoalCompleted(LevelGoal.GoalType goalType)
    {
        CheckAndUpdateGoalItems(goalType, 0);
    }

    // iterate over the goal items array, and if the goal type matches, refresh that item with the new counts
    private void CheckAndUpdateGoalItems(LevelGoal.GoalType goalType, int newCount)
    {
        for (int index = 0; index < goalCount; index++)
        {
            if (goalItems[index].MyGoalType == goalType)
            {
                goalItems[index].RefreshGoalItem(newCount);
                break; // only one goal will match the goal type
            }
        }
    }

    // helper function to select the sprite from the available options based on the input goal type
    private Sprite GetSpriteBasedOnGoalType(LevelGoal.GoalType goalType)
    {
        switch (goalType)
        {
            case LevelGoal.GoalType.CollectRed:
                return goalSpriteOptions[0];
            
            case LevelGoal.GoalType.CollectGreen:
                return goalSpriteOptions[1];
            
            case LevelGoal.GoalType.CollectBlue:
                return goalSpriteOptions[2];
            
            case LevelGoal.GoalType.CollectYellow:
                return goalSpriteOptions[3];
            
            case LevelGoal.GoalType.CollectOrange:
                return goalSpriteOptions[4];
            
            case LevelGoal.GoalType.CollectViolet:
                return goalSpriteOptions[5];
            
            case LevelGoal.GoalType.CollectAny:
                return goalSpriteOptions[6];
        }

        return null;
    }
    
    
}
