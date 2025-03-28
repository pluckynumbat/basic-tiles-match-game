using System.Collections.Generic;
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
    
    // raised by the level manager when the level scene is loaded
    public delegate void LevelDataRequestHandler();
    public static event LevelDataRequestHandler LevelDataRequestEvent;
    public static void RaiseLevelDataRequestEvent()
    {
        LevelDataRequestEvent?.Invoke();
    }
    
    // raised by the main manager in response to a level data requested event
    public delegate void LevelDataReadyHandler(LevelData levelData);
    public static event LevelDataReadyHandler LevelDataReadyEvent;
    public static void RaiseLevelDataReadyEvent(LevelData levelData)
    {
        LevelDataReadyEvent?.Invoke(levelData);
    }
    
    // raised by the game grid manager when the initial setup for the game grid is done
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
    
    // raised by the game grid manager during a move, when grid cells have been marked as collected
    public delegate void GridCellsCollectedHandler(List<GameGridCell> gridCellsCollected);
    public static event GridCellsCollectedHandler GridCellsCollectedEvent;
    public static void RaiseGridCellsCollectedEvent(List<GameGridCell> gridCellsCollected)
    {
        GridCellsCollectedEvent?.Invoke(gridCellsCollected);
    }
    
    // raised by the game grid manager during a move, when grid cells have been 'removed' from the grid (marked as empty)
    public delegate void GridCellsRemovedHandler(List<GameGridCell> gridCellsRemoved);
    public static event GridCellsRemovedHandler GridCellsRemovedEvent;
    public static void RaiseGridCellsRemovedEvent(List<GameGridCell> gridCellsRemoved)
    {
        GridCellsRemovedEvent?.Invoke(gridCellsRemoved);
    }
    
    // raised by the game grid manager during a move, when existing grid cells are assigned to 'fill holes' in the grid
    public delegate void GridCellsFillHolesHandler(List<GameGridCell> cellsThatFillHoles, int[][] holesBelowCells);
    public static event GridCellsFillHolesHandler GridCellsFillHolesEvent;
    public static void RaiseGridCellsFillHolesEvent(List<GameGridCell> cellsThatFillHoles, int[][] holesBelowCells)
    {
        GridCellsFillHolesEvent?.Invoke(cellsThatFillHoles, holesBelowCells);
    }
    
    // raised by the game grid manager when a refill grid has been populated with cells in hole positions of the main grid,
    // and a new holes below cells has been calculated for the refill grid
    public delegate void RefillGridReadyHandler(GameGridCell[][] refillGrid, int[][] holesBelowCells);
    public static event RefillGridReadyHandler RefillGridReadyEvent;
    public static void RaiseRefillGridReadyEvent(GameGridCell[][] refillGrid, int[][] holesBelowCells)
    {
        RefillGridReadyEvent?.Invoke(refillGrid, holesBelowCells);
    }
    
    // raised by the game tiles manager when the last new tile has reached
    // its final destination in the main grid, signalling the end of a move
    public delegate void MoveCompletedHandler();
    public static event MoveCompletedHandler MoveCompletedEvent;
    public static void RaiseMoveCompletedEvent()
    {
        MoveCompletedEvent?.Invoke();
    }
    
    // raised by the level goals manager during a move, when a goal's progress is updated
    public delegate void GoalProgressUpdatedHandler(LevelGoal.GoalType goalType, int remaining);
    public static event GoalProgressUpdatedHandler GoalProgressUpdatedEvent;
    public static void RaiseGoalProgressUpdatedEvent(LevelGoal.GoalType goalType, int remaining)
    {
        GoalProgressUpdatedEvent?.Invoke(goalType, remaining);
    }
    
    // raised by the level goals manager during a move, when a new goal is completed
    public delegate void GoalCompletedHandler(LevelGoal.GoalType goalType);
    public static event GoalCompletedHandler GoalCompletedEvent;
    public static void RaiseGoalCompletedEvent(LevelGoal.GoalType goalType)
    {
        GoalCompletedEvent?.Invoke(goalType);
    }
    
    // raised by the level manager when the level ends
    public delegate void LevelEndedHandler(bool won);
    public static event LevelEndedHandler LevelEndedEvent;
    public static void RaiseLevelEndedEvent(bool won)
    {
        LevelEndedEvent?.Invoke(won);
    }
     
    // raised by the game grid manager when the main grid is shuffled
    public delegate void GridShuffledHandler(GameGridCell[][] shuffledGrid);
    public static event GridShuffledHandler GridShuffledEvent;
    public static void RaiseGridShuffledEvent(GameGridCell[][] shuffledGrid)
    {
        GridShuffledEvent?.Invoke(shuffledGrid);
    }
}
