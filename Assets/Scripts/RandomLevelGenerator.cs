using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static Class that generates a random level (more specifically a random LevelData object) within certain parameters
/// </summary>
public static class RandomLevelGenerator
{
    // min and max values for the different colors in a level
    private const int MIN_COLOR_COUNT = 4;
    private const int MAX_COLOR_COUNT = 6;
    
    // min and max grid lengths (5x5 gird to 9x9 grid)
    private const int MIN_GRID_LENGTH = 5;
    private const int MAX_GRID_LENGTH = 9;
    
    //min and max starting move counts
    private const int MIN_MOVE_COUNT = 10;
    private const int MAX_MOVE_COUNT = 50;
    
    //min and max goal counts
    private const int MIN_GOAL_COUNT = 1;
    private const int MAX_GOAL_COUNT = 4;
    
    //min and max goal amount ranges
    private const int MIN_GOAL_AMOUNT = 1;
    private const int MAX_GOAL_AMOUNT = 20;
    
    // create and return a new LevelData object which contains randomized level parameters
    public static LevelData GenerateRandomLevel()
    {
        LevelData randomLevel = new LevelData();
        randomLevel.isStartingGridFixed = false;
        
        // these are straight forward, just assign numbers within allowed ranges
        randomLevel.colorCount = Random.Range(MIN_COLOR_COUNT, MAX_COLOR_COUNT + 1);
        randomLevel.gridLength = Random.Range(MIN_GRID_LENGTH, MAX_GRID_LENGTH + 1);
        randomLevel.startingMoveCount = Random.Range(MIN_MOVE_COUNT, MAX_MOVE_COUNT + 1);

        // select 'colorCount' number of colors randomly to form the color list for the level
        randomLevel.colorPalette = new List<string>();
        List<string> possibleCellColors = GetPossibleCellColors();
        for (int index = 0; index < randomLevel.colorCount; index++)
        {
            // pick a cell color from the possible candidates for the level
            int cellColorRoll = Random.Range(0, possibleCellColors.Count);
            
            //add it to the color palette
            randomLevel.colorPalette.Add(possibleCellColors[cellColorRoll]);
            
            //remove the cell color from further consideration
            possibleCellColors.RemoveAt(cellColorRoll);
        }
        
        // select (1-4) goals randomly and generate new levelGoatData objects from them 
        int goalCount = Random.Range(MIN_GOAL_COUNT, MAX_GOAL_COUNT + 1);
        randomLevel.goals = new List<LevelGoalData>();
        List<string> possibleGoalTypes = GetPossibleGoalTypes(randomLevel.colorCount);
        for (int goalIndex = 0; goalIndex < goalCount; goalIndex++)
        {
            LevelGoalData levelGoalData = new LevelGoalData();

            // pick a goal type from the possible candidates for the level
            int goalTypeRoll = Random.Range(0, possibleGoalTypes.Count);
            levelGoalData.goalType = possibleGoalTypes[goalTypeRoll];
            levelGoalData.goalAmount = Random.Range(MIN_GOAL_AMOUNT, MAX_GOAL_AMOUNT + 1);
            randomLevel.goals.Add(levelGoalData);
            
            //remove the goal type from further consideration (no duplicate goals allowed)
            possibleGoalTypes.RemoveAt(goalTypeRoll);
            
        }

        return randomLevel;
    }
    
    // create a list of possible candidates for the color palette to choose from
    private static List<string> GetPossibleCellColors()
    {
        List<string> possibleCellColors = new List<string>();
        
        // add cell colors to the palette for all colors
        for (int index = 0; index < MAX_COLOR_COUNT; index++)
        {
            string cellColorString = GameGridCell.GetGridCellStringFromColor((GameGridCell.GridCellColor)index);
            possibleCellColors.Add(cellColorString);
        }
        return possibleCellColors;
    }

    // goal types depend on allowed color count, create a list of possible candidates
    private static List<string> GetPossibleGoalTypes(int colorCount)
    {
        List<string> possibleGoalTypes = new List<string>();
        
        // add collect goals for all possible colors
        for (int index = 0; index < colorCount; index++)
        {
            string goalTypeString = LevelGoal.GetGoalTypeStringFromGoalType((LevelGoal.GoalType)index);
            possibleGoalTypes.Add(goalTypeString);
        }
        
        // add the collect any goal
        possibleGoalTypes.Add(LevelGoal.GetGoalTypeStringFromGoalType(LevelGoal.GoalType.CollectAny));
        return possibleGoalTypes;
    }
}
