using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that parses a json files representing level and provides level data that other systems can use
/// </summary>
public class LevelJSONReader : MonoBehaviour
{
    private const string LEVEL_DATA_DIRECTORY = "LevelData/";
    private const string DEFAULT_LEVEL_NAME = "testLevel7x7"; // default level that is used as fallback in case we do not find level to load
    
    public static LevelData ReadJSON(string fileName, string defaultFile = DEFAULT_LEVEL_NAME)
    {
        string path = LEVEL_DATA_DIRECTORY + fileName;
        string defaultPath = LEVEL_DATA_DIRECTORY + defaultFile;
        
        var jsonLevelFile = Resources.Load<TextAsset>(path);

        if (jsonLevelFile == null)
        {
            Debug.LogError($"requested level file with file name: '{fileName}' does not exist!, falling back to default level");
            jsonLevelFile = Resources.Load<TextAsset>(defaultPath);
        }

        LevelData levelData = JsonUtility.FromJson<LevelData>(jsonLevelFile.ToString());

        return levelData;
    }
    
    public static LevelData CreateLevelDataFromJSONString(string jsonString)
    {
        LevelData levelData = JsonUtility.FromJson<LevelData>(jsonString);
        return levelData;
    }
}

// Data structure to represent level data from a level file
[System.Serializable]
public class LevelData
{
    public string name; // name of the level
    public int seed; // optional ability to provide a fixed seed. If not provided, it defaults to 0, and the game will ignore it (level will not use fixed seed)
    public int colorCount; // number of different colors that the gird can be populated with
    public List<string> colorPalette; // list of colors to construct the grid from (e.g. "R" ,"G", "B", "Y") the length should match the colorCount above
    public int gridLength; // length of 1 dimension of the square grid (e.g. length 6 means the grid is 6x6)
    public bool isStartingGridFixed; // is the starting grid fixed (if so, the following property should be specified in the level file)
    public List<string> startingGrid; // specification of the starting grid, if fixed
    public List<LevelGoalData> goals; // list of goals for the level
    public int startingMoveCount; // moves the player has to finish the level
}

[System.Serializable]
public class LevelGoalData
{
    public string goalType; // type of goal (e.g. goal type "R" might represent having to collect red tiles)
    public int goalAmount; // amount required to fulfill this goal
}

