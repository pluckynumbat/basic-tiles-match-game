using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represent a single goal in the level
/// </summary>
public class LevelGoal
{
    public enum GoalType
    {
        None = -1,
        CollectRed,
        CollectGreen,
        CollectBlue,
        CollectYellow,
        CollectOrange,
        CollectViolet,
        CollectAny, // not tied to a particular color
    }

    public readonly GoalType Type; // type of the goal from the above enums
    public readonly int TotalAmount; // total requirement to fulfill this goal
    public int Remaining; // how many of this goal do we still need?
    
    public LevelGoal(GoalType type, int total)
    {
        Type = type;
        TotalAmount = total;
        Remaining = total;
    }
    
    //helper to get goal type from a string in the level data
    public static GoalType GetGoalTypeFromString(string goalString)
    {
        switch (goalString)
        {
            case "R":
                return GoalType.CollectRed;
            
            case "G":
                return GoalType.CollectGreen;
            
            case "B":
                return GoalType.CollectBlue;
            
            case "Y":
                return GoalType.CollectYellow;
            
            case "O":
                return GoalType.CollectOrange;
            
            case "V":
                return GoalType.CollectViolet;
            
            case "A":
                return GoalType.CollectAny;
        }
        
        Debug.LogError($"Invalid goal string: {goalString}");
        return GoalType.None;
    }
    
        
    //helper to get goal type string from goal type
    public static string GetGoalTypeStringFromGoalType(GoalType goalType)
    {
        switch (goalType)
        {
            case GoalType.CollectRed:
                return "R";
            
            case GoalType.CollectGreen:
                return "G";
            
            case GoalType.CollectBlue:
                return "B";
            
            case GoalType.CollectYellow:
                return "Y";
            
            case GoalType.CollectOrange:
                return "O";
            
            case GoalType.CollectViolet:
                return "V";
            
            case GoalType.CollectAny:
                return "A";
        }
        return "A";
    }
    
    // helper to get goal type from a Grid cell color type
    public static GoalType GetGoalTypeFromGridCellColor(GameGridCell.GridCellColor color)
    {
        switch (color)
        {
            case GameGridCell.GridCellColor.Red:
                return GoalType.CollectRed;
            
            case GameGridCell.GridCellColor.Green:
                return GoalType.CollectGreen;
            
            case GameGridCell.GridCellColor.Blue:
                return GoalType.CollectBlue;
            
            case GameGridCell.GridCellColor.Yellow:
                return GoalType.CollectYellow;
            
            case GameGridCell.GridCellColor.Orange:
                return GoalType.CollectOrange;
            
            case GameGridCell.GridCellColor.Violet:
                return GoalType.CollectViolet;
        }
        
        Debug.LogError($"Goal type does not exist for goal color: {color}");
        return GoalType.None;
    }
}
