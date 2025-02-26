using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the functionality related to creating and modifying the game grid(s)
/// which are the data / Model part of the game board
/// </summary>
public class GameGridManager : MonoBehaviour
{
    private GameGridCell[][] mainGrid; // the main / active grid in the level
    
    private int gridLength; // both height and width of the grid are the same, store in this variable

    private void Awake()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.LevelDataReadyEvent += OnLevelDataReady;
    }

    private void OnDestroy()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
    }

    private void OnLevelDataReady(LevelData data)
    {
        SetupGameGrid(data);
    }
    
    // does the initial setup of the game grid when a new level begins
    private void SetupGameGrid(LevelData levelData)
    {
        gridLength = levelData.gridLength;

        bool isGridRandom = levelData.isStartingGridRandom;
        
        mainGrid = new GameGridCell[gridLength][];
        for (int y = 0; y < gridLength; y++)
        {
            mainGrid[y] = new GameGridCell[gridLength];
            for (int x = 0; x < gridLength; x++)
            {
                mainGrid[y][x] = new GameGridCell(y, x);
                if (isGridRandom)
                {
                    mainGrid[y][x].Color = GetRandomGridCellColor(levelData.colorCount);
                }
                else // get the grid specified in the level data
                {
                    //mapping from a top first list, to a bottom first 2D array
                    int mapping =  ((gridLength - 1 - y) * gridLength) + x;
                    mainGrid[y][x].Color = GetGridCellColorFromString(levelData.startingGrid[mapping]);
                }
                mainGrid[y][x].Occupied = true;
            }
        }
    }
    
    // randomly generate a new grid cell color
    private GameGridCell.GridCellColor GetRandomGridCellColor(int validColorCount)
    {
        //TODO init seed?
        int randomRoll = Random.Range(0, validColorCount);
        return (GameGridCell.GridCellColor)randomRoll;
    }

    //helper to select color based on string input
    private GameGridCell.GridCellColor GetGridCellColorFromString(string colorString)
    {
        switch (colorString)
        {
            case "R":
                return GameGridCell.GridCellColor.Red;
            
            case "G":
                return GameGridCell.GridCellColor.Green;
            
            case "B":
                return GameGridCell.GridCellColor.Blue;
            
            case "Y":
                return GameGridCell.GridCellColor.Yellow;
        }
        
        Debug.LogError($"Invalid color string: {colorString}");
        return GameGridCell.GridCellColor.None;
    }
}
