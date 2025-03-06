using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// New editor tool to perform basic validation on a given level data object
/// NOTE: this tool can be run in the editor by clicking 'Assets->Validate All Level Files'
/// </summary>
public static class LevelDataValidator
{
    private const string LEVEL_DATA_DIRECTORY = "LevelData/";
    
    // min and max values for the different colors in a level
    private const int MIN_COLOR_COUNT = 4;
    private const int MAX_COLOR_COUNT = 6;
    
    // min and max grid lengths (5x5 gird to 9x9 grid)
    private const int MIN_GRID_LENGTH = 5;
    private const int MAX_GRID_LENGTH = 9;
    
    //min and max goal counts
    private const int MIN_GOAL_COUNT = 1;
    private const int MAX_GOAL_COUNT = 4;

    private static HashSet<string> gridEntriesUniversalSet =  new HashSet<string>() { "R", "G", "B", "Y", "O", "V"};

    public static void ValidateAllLevelFiles()
    {
#if UNITY_EDITOR
        bool noIssues = true;
        var jsonLevelFiles = Resources.LoadAll<TextAsset>(LEVEL_DATA_DIRECTORY);
        foreach (TextAsset jsonLevelFile in jsonLevelFiles)
        {
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonLevelFile.ToString());
            noIssues &= IsLevelDataValid(levelData);
        }

        if (noIssues)
        {
            Debug.Log($"All level files are valid!");
        }
#endif
    }

    public static bool IsLevelDataValid(LevelData levelData)
    {
        if (levelData == null) // basic failure
        {
            Debug.LogError($"LevelData object created from the level was null");
            return false;
        }
        
        Debug.Log($"Checking the level data with name: {levelData.name}");

        // allowed color count is (4-6)
        if (levelData.colorCount < MIN_COLOR_COUNT || levelData.colorCount > MAX_COLOR_COUNT)
        {
            Debug.LogError($"Invalid color count for the level. Min. value is {MIN_COLOR_COUNT}, Max. value is {MAX_COLOR_COUNT}, current value is: {levelData.colorCount}");
            return false;
        }
        
        // color palette must be specified, and its length should be as long as the color count
        if (levelData.colorPalette.Count != levelData.colorCount)
        {
            Debug.LogError($"Invalid color palette count for the level: {levelData.colorPalette.Count}, it must match the level's color count: {levelData.colorCount}");
            return false;
        }
        
        //each entry in the color palette must be a valid grid entry, and no entries can be repeated
        HashSet<string> allowedColorPaletteEntries = new HashSet<string>(gridEntriesUniversalSet);
        foreach (string entry in levelData.colorPalette)
        {
            
            if (!allowedColorPaletteEntries.Contains(entry))
            {
                Debug.LogError($"Color Palette contains an invalid entry: {entry}, it might be an invalid string, or it might be used already. please check the level file");
                return false;
            }
            else
            {
                allowedColorPaletteEntries.Remove(entry); // color palette entries cannot be repeated
            }
        }
        
        // allowed grid length is (5-9)
        if (levelData.gridLength < MIN_GRID_LENGTH || levelData.gridLength > MAX_GRID_LENGTH)
        {
            Debug.LogError($"Invalid grid length for the level. Min. value is {MIN_GRID_LENGTH}, Max. value is {MAX_GRID_LENGTH}, current value is: {levelData.gridLength}");
            return false;
        }
        
        // if the starting grid is fixed, it should be specified and have valid entries
        if (levelData.isStartingGridFixed)
        {
            if (levelData.startingGrid == null || levelData.startingGrid.Count == 0)
            {
                Debug.LogError($"Level is marked as having a fixed starting gird, but the starting grid has not been specified, or is empty");
                return false;
            }
            
            // the starting grid list should have a length of (grid length^2)
            if (levelData.startingGrid.Count != levelData.gridLength * levelData.gridLength)
            {
                Debug.LogError($"Starting grid should have exactly {levelData.gridLength * levelData.gridLength} entries for the given grid length of {levelData.gridLength}, current number of entries is : {levelData.startingGrid.Count}");
                return false;
            }

            // check each entry in the list, it should be part of the color palette
            HashSet<string> allowedGridEntries = new HashSet<string>(levelData.colorPalette);
            for (int index = 0; index < levelData.startingGrid.Count; index++)
            {
                if (!allowedGridEntries.Contains(levelData.startingGrid[index]))
                {
                    Debug.LogError($"Starting grid has an invalid entry: {levelData.startingGrid[index]} at index {index} ");
                    return false;
                }
            }
        }
        
        // goal count
        if (levelData.goals == null || levelData.goals.Count < MIN_GOAL_COUNT || levelData.goals.Count > MAX_GOAL_COUNT)
        {
            Debug.LogError($"List of goals is either null or has invalid Count: {levelData.goals?.Count}");
            return false;
        }
        
        // check if each goal is allowed based on color palette, is not repeated, and has a valid goal amount (greater than 0)
        HashSet<string> allowedGoalTypeEntries = new HashSet<string>();
        
        // collect all goal types based on colors in the color palette
        foreach (string cellColorString in levelData.colorPalette)
        {
            GameGridCell.GridCellColor cellColor = GameGridCell.GetGridCellColorFromString(cellColorString);
            LevelGoal.GoalType goalType = LevelGoal.GetGoalTypeFromGridCellColor(cellColor);
            allowedGoalTypeEntries.Add(LevelGoal.GetGoalTypeStringFromGoalType(goalType));
        }

        //collect any is always allowed
        allowedGoalTypeEntries.Add(LevelGoal.GetGoalTypeStringFromGoalType(LevelGoal.GoalType.CollectAny));
        
        foreach (LevelGoalData goalData in levelData.goals)
        {
            if (goalData == null)
            {
                Debug.LogError("Null entry found in list of goals");
                return false;
            }

            if (!allowedGoalTypeEntries.Contains(goalData.goalType))
            {
                Debug.LogError($"Goals list contains an invalid entry: {goalData.goalType}, it might not be allowed because of color count, or might have been used as a goal already. please check the level file");
                return false;
            }
            else
            {
                allowedGoalTypeEntries.Remove(goalData.goalType); // goal types cannot be repeated
            }

            if (goalData.goalAmount < 0)
            {
                Debug.LogError($"Goal amount for any goal cannot be zero, current value (for goal of type {goalData.goalType}) is: {goalData.goalAmount}");
                return false;
            }
        }
        
        // starting move count should be more than 0
        if (levelData.startingMoveCount < 0)
        {
            Debug.LogError($"Starting move count of a level cannot be less than zero, current value is: {levelData.startingMoveCount}");
            return false;
        }
        
        Debug.Log($"Level data for level with name {levelData.name} is valid!");
        return true;
    }
}