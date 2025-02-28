using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// UI script to display moves during a level
/// </summary>
public class UIMovesDisplay : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    private int movesCount; // number of moves left
    private void Awake()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.LevelDataReadyEvent += OnLevelDataReady;
        
        GameEvents.MoveCompletedEvent -= OnMoveCompleted;
        GameEvents.MoveCompletedEvent += OnMoveCompleted;
    }

    private void OnDestroy()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.MoveCompletedEvent -= OnMoveCompleted;
    }

    // initialize move count, update display
    private void OnLevelDataReady(LevelData levelData)
    {
        movesCount = levelData.startingMoveCount;
        RefreshMovesDisplay();
    }
    
    // reduce move count, update display
    private void OnMoveCompleted()
    {
        if (movesCount <= 0)
        {
            return;
        }

        movesCount -= 1;
        RefreshMovesDisplay();
    }

    //update the text based on current move count
    private void RefreshMovesDisplay()
    {
        textBox.text = movesCount.ToString();
    }
}
