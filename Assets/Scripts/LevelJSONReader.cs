using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that parses a json files representing level and provides level data that other systems can use
/// </summary>
public class LevelJSONReader : MonoBehaviour
{
    private const string LEVEL_DATA_DIRECTORY = "LevelData/";
    
    public static LevelData ReadJSON(string fileName)
    {
        string path = LEVEL_DATA_DIRECTORY + fileName;
        
        var jsonLevelFile = Resources.Load<TextAsset>(path);

        LevelData levelData = JsonUtility.FromJson<LevelData>(jsonLevelFile.ToString());

        return levelData;
    }
}

// Data structure to represent level data from a level file
[System.Serializable]
public class LevelData
{
    public string name; // name of the level
    public int gridLength; // length of 1 dimension of the square grid (e.g. length 6 means the grid is 6x6)
    public bool isStartingGridRandom; // should the starting grid be random?
    public List<string> startingGrid; // specification of the starting grid, if not random
    public List<LevelGoal> goals; // list of goals for the level
    public int startingMoveCount; // moves the player has to finish the level
}

[System.Serializable]
public class LevelGoal
{
    public string goalType; // type of goal (e.g. goal type "R" might represent having to collect red tiles)
    public int goalAmount; // amount required to fulfill this goal
}

