using UnityEngine;

/// <summary>
/// Static class to hold all the different game events that can be raised in a session
/// </summary>
public static class GameEvents
{
    // raised by the input detector when player input is detected
    public delegate void InputDetectedHandler(Vector3 inputWorldPosition);
    public static event InputDetectedHandler InputDetectedEvent;
    public static void RaiseInputDetectedEvent(Vector3 inputWorldPosition)
    {
        InputDetectedEvent?.Invoke(inputWorldPosition);
    }
    
    // raised by the level manager when level data is loaded from a file and ready
    public delegate void LevelDataReadyHandler(LevelData levelData);
    public static event LevelDataReadyHandler LevelDataReadyEvent;
    public static void RaiseLevelDataReadyEvent(LevelData levelData)
    {
        LevelDataReadyEvent?.Invoke(levelData);
    }
    
    // raised by the game grid manager when the intiaal setup for the game grid is done
    public delegate void GameGridReadyHandler(GameGridCell[][] gameGrid);
    public static event GameGridReadyHandler GameGridReadyEvent;
    public static void RaiseGameGridReadyEvent(GameGridCell[][] gameGrid)
    {
        GameGridReadyEvent?.Invoke(gameGrid);
    }
    
    // raised by the game tiles manager when the player interacts with an active tile
    public delegate void ActiveTileTappedHandler(int gridY, int gridX);
    public static event ActiveTileTappedHandler ActiveTileTappedEvent;
    public static void RaiseActiveTileTappedEvent(int gridY, int gridX)
    {
        ActiveTileTappedEvent?.Invoke(gridY, gridX);
    }
    
    // raised by the game grid manager when the player makes any sort of invalid move
    public delegate void InvalidMoveEventHandler(int gridY, int gridX);
    public static event InvalidMoveEventHandler InvalidMoveEvent;
    public static void RaiseInvalidMoveEvent(int gridY, int gridX)
    {
        InvalidMoveEvent?.Invoke(gridY, gridX);
    }
}
