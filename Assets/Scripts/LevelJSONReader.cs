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
}
