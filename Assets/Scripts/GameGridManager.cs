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
    }

    private void OnDestroy()
    {
    }
}
