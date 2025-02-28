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
}
