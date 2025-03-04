// TODO: remove this later if not required
// Uncomment the following line to enable grid related logs
#define GAME_GRID_LOGGING

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the functionality related to creating and modifying the game grid(s)
/// which are the data / Model part of the game board
/// </summary>
public class GameGridManager : MonoBehaviour
{
    private const int SEED_TO_IGNORE = 0; // this is the default value of the level data seed, and this value means that it will not be used
    
    private GameGridCell[][] mainGrid; // the main / active grid in the level

    private GameGridCell[][] refillGrid; // a grid used to refill the main grid in the last part of the player's move
    
    private int gridLength; // both height and width of the grid are the same, store in this variable

    private bool[][] visited; // 2d array used during the Breadth First Search to check and mark if a grid cell has been visited already

    private int[][] holesBelowCells; // 2d array which for each location in the given grid, stores the count of holes below that cell's location

    private int validColorCount; // stores the amount of different colors available for the cells in a given level

    private void Awake()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.LevelDataReadyEvent += OnLevelDataReady;
        
        GameEvents.ActiveTileTappedEvent -= OnActiveTileTapped;
        GameEvents.ActiveTileTappedEvent += OnActiveTileTapped;
    }

    private void OnDestroy()
    {
        GameEvents.LevelDataReadyEvent -= OnLevelDataReady;
        GameEvents.ActiveTileTappedEvent -= OnActiveTileTapped;
    }

    private void OnLevelDataReady(LevelData data)
    {
        SetupGameGrid(data);
        InitializeRefillGrid();
        SetupOtherDataStructures();
        GameEvents.RaiseGameGridReadyEvent(mainGrid);
    }
    
    // does the initial setup of the game grid when a new level begins
    private void SetupGameGrid(LevelData levelData)
    {
        if (levelData.seed != SEED_TO_IGNORE)
        {
            Random.InitState(levelData.seed);
        }

        validColorCount = levelData.colorCount;
        gridLength = levelData.gridLength;

        bool isStartGridFixed = levelData.isStartingGridFixed;
        
        mainGrid = new GameGridCell[gridLength][];
        for (int y = 0; y < gridLength; y++)
        {
            mainGrid[y] = new GameGridCell[gridLength];
            for (int x = 0; x < gridLength; x++)
            {
                mainGrid[y][x] = new GameGridCell(y, x);
                if (isStartGridFixed) // get the grid specified in the level data
                {
                    //mapping from a top first list, to a bottom first 2D array
                    int mapping =  ((gridLength - 1 - y) * gridLength) + x;
                    mainGrid[y][x].Color = GetGridCellColorFromString(levelData.startingGrid[mapping]);
                }
                else // fill the starting grid randomly (controlled by the seed if specified)
                {
                    mainGrid[y][x].Color = GetRandomGridCellColor(validColorCount);
                }
                mainGrid[y][x].Occupied = true;
            }
        }
#if GAME_GRID_LOGGING
        PrintGridToConsole(mainGrid);
#endif
    }
    
    // the refill grid should be set to all empty at the beginning of a level
    private void InitializeRefillGrid()
    {
        refillGrid = new GameGridCell[gridLength][];
        for (int y = 0; y < gridLength; y++)
        {
            refillGrid[y] = new GameGridCell[gridLength];
            for (int x = 0; x < gridLength; x++)
            {
                refillGrid[y][x] = new GameGridCell(y,x);
                refillGrid[y][x].Color = GameGridCell.GridCellColor.None;
                refillGrid[y][x].Occupied = false;
            }
        }
    }
    
    // set up the different helper data structures that will be used during grid processing
    private void SetupOtherDataStructures()
    {
        // create the visited array once
        visited = new bool[gridLength][];
        for (int y = 0; y < gridLength; y++)
        {
            visited[y] = new bool[gridLength];
        }
        
        // create the holes below cells array once
        holesBelowCells = new int[gridLength][];
        for (int y = 0; y < gridLength; y++)
        {
            holesBelowCells[y] = new int[gridLength];
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
            
            case "O":
                return GameGridCell.GridCellColor.Orange;
            
            case "V":
                return GameGridCell.GridCellColor.Violet;
        }
        
        Debug.LogError($"Invalid color string: {colorString}");
        return GameGridCell.GridCellColor.None;
    }
    
    // player attempted a move on the game board, process the move
    private void OnActiveTileTapped(int gridY, int gridX)
    {
        // check if the grid position is valid, and grid item is valid
        if (!IsWithinGridBounds(gridY, gridX))
        {
            GameEvents.RaiseInvalidMoveEvent(gridY, gridX);
            Debug.LogError($"Invalid input in OnActiveTileTapped (out of bounds), x: {gridX}, y: {gridY}");
            return;
        }

        // get the cell at the tapped tile's location in the grid
        GameGridCell tappedCell = mainGrid[gridY][gridX];
        if (!tappedCell.Occupied || tappedCell.Color == GameGridCell.GridCellColor.None)
        {
            GameEvents.RaiseInvalidMoveEvent(gridY, gridX);
            Debug.LogError($"Invalid input in OnActiveTileTapped (cell is empty), x: {gridX}, y: {gridY}");
            return;
        }
        
#if GAME_GRID_LOGGING
        //TODO: remove this log later? 
        Debug.Log($"Active tile tapped: X: {gridX}, Y: {gridY}");
#endif
        
        // Start actual processing
        
        //1. Check if the grid cell has neighbors with the same color
        if (!AnyNeighborWithSameColor(gridY, gridX, tappedCell.Color, mainGrid))
        {
            // if not, raise invalid move event, and return
            GameEvents.RaiseInvalidMoveEvent(gridY, gridX);
            //TODO: some feedback here or in the tiles manager to let the player know that a single tile cannot be removed?
            Debug.Log($"(single cell cannot be removed), x: {gridX}, y: {gridY}");
            return;
        }
        
        //2. Grid Cell 'Collection'
        
        //2a. Collect all neighbors with the same color using a Breadth First Search starting at the cell at the tapped tile's location
        List<GameGridCell> gridCellsToRemove = CollectNeighborsWithSameColor(tappedCell, mainGrid);
        
        //2b. Let other systems know that these cells have been 'collected' so they can deal with the information
        // (e.g. level goals manager will update goal progress based on this)
        GameEvents.RaiseGridCellsCollectedEvent(gridCellsToRemove);
        
        //3. Grid Cell 'Removal'
        
        //3a. Mark all the cells to remove as empty
        foreach (GameGridCell cell in gridCellsToRemove)
        {
            cell.Occupied = false;
            cell.Color = GameGridCell.GridCellColor.None;
        }
        
        //3b. Let other systems know that these cells have been 'removed' so they can deal with the information
        // (e.g. tiles manager will have to actually remove tiles based on this list)
        GameEvents.RaiseGridCellsRemovedEvent(gridCellsToRemove);
        
        //4. Fill holes with existing cells
        
        //4a. for each cell location in the main grid, calculate the number of holes below it
        holesBelowCells = CalculateHolesBelowCells(mainGrid);
        
        //4b. collect all grid cells with holes below them which are not holes themselves
        // these will be used to fill the holes
        List<GameGridCell> cellsThatFillHoles = CollectGridCellsThatFillHoles(mainGrid);
        
        //4c. Let other systems know that these cells will fill existing holes
        // along with the holesBelowCells array (which has the hole amounts)
        //(e.g. this will be used by the tile manager to actually move the tiles)
        GameEvents.RaiseGridCellsFillHolesEvent(cellsThatFillHoles, holesBelowCells);
        
        //4d. swap holes with the cells that will fill them (based on the holesBelowCells values)
        SwapHolesWithExistingCells(cellsThatFillHoles, mainGrid);
        
#if GAME_GRID_LOGGING
        PrintGridToConsole(mainGrid);
#endif
        
        //5. Bring in the 'new' cells

        //5a. populate the refill grid with cells only at hole positions in main grid
        PopulateRefillGrid(mainGrid);
        
#if GAME_GRID_LOGGING
        PrintGridToConsole(refillGrid);
#endif    
        
        //5b. create a holesBelowCells for this refill grid
        // (this is needed for the new tiles to reposition themselves before dropping in)
        holesBelowCells = CalculateHolesBelowCells(refillGrid);
        
        //5c. send the refill Grid and the new 'holesBelowCells' array to other systems
        // (e.g. the tiles manager will use these to inform the starting positions of the new tiles
        // as well as their end destinations to be moved to)
        GameEvents.RaiseRefillGridReadyEvent(refillGrid, holesBelowCells);
        
        //5d. finally, set the empty cells to in the main grid to the colors of the ones in the refill grid (these will be the final cells)
        CompleteMainGridUsingRefillGrid();
    }
    
    // helper function to check if given y and x co-ordinates are valid for the game grid(s)
    private bool IsWithinGridBounds(int y, int x)
    {
        return  0 <= y && y < gridLength && x >= 0 && x <gridLength;
    }
    
    // helper function to check for a given cell Y and X indices, if there exists at least 1 neighbor with the same color in the given grid
    private bool AnyNeighborWithSameColor(int y, int x, GameGridCell.GridCellColor color, GameGridCell[][] grid)
    {
        // north neighbor
        if (IsWithinGridBounds(y + 1, x) && grid[y + 1][x].Color == color)
        {
            return true;
        }
        
        // east neighbor
        if (IsWithinGridBounds(y, x + 1) && grid[y][x + 1].Color == color)
        {
            return true;
        }
        
        // south neighbor
        if (IsWithinGridBounds(y - 1, x) && grid[y - 1][x].Color == color)
        {
            return true;
        }
        
        // west neighbor
        if (IsWithinGridBounds(y, x - 1) && grid[y][x - 1].Color == color)
        {
            return true;
        }

        return false;
    }
    
    //Collect all neighbors with the same color using a Breadth First Search starting at the cell at the tapped tile's location
    private List<GameGridCell> CollectNeighborsWithSameColor(GameGridCell tappedCell, GameGridCell[][] grid)
    {
        List<GameGridCell> relevantCells = new List<GameGridCell>(); // these will be sent to the caller at the end of the method

        // reset the visited array
        for (int y = 0; y < gridLength; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                visited[y][x] = false;
            } 
        }
        
        // create a queue and add the starting cell to it
        Queue<GameGridCell> queue = new Queue<GameGridCell>();
        queue.Enqueue(tappedCell);

        while (queue.Count > 0)
        {
            // get the next cell to process
            GameGridCell current = queue.Dequeue();
            
            // skip if visited already
            if (visited[current.Y][current.X])
            {
                continue;
            }

            // mark as visited and add to list of relevant cells
            visited[current.Y][current.X] = true;
            relevantCells.Add(current);
            
            // check and visit neighbors (within grid bounds) with same color that are unvisited
            
            // north neighbor
            if (IsWithinGridBounds(current.Y + 1, current.X) &&
                grid[current.Y + 1][current.X].Color == tappedCell.Color &&
                !visited[current.Y + 1][current.X])
            {
                queue.Enqueue(grid[current.Y + 1][current.X]);
            }
            
            // east neighbor
            if (IsWithinGridBounds(current.Y, current.X + 1) &&
                grid[current.Y][current.X + 1].Color == tappedCell.Color &&
                !visited[current.Y][current.X + 1])
            {
                queue.Enqueue(grid[current.Y][current.X + 1]);
            }
            
            // south neighbor
            if (IsWithinGridBounds(current.Y - 1, current.X) &&
                grid[current.Y - 1][current.X].Color == tappedCell.Color &&
                !visited[current.Y - 1][current.X])
            {
                queue.Enqueue(grid[current.Y - 1][current.X]);
            }
            
            // west neighbor
            if (IsWithinGridBounds(current.Y, current.X - 1) &&
                grid[current.Y][current.X - 1].Color == tappedCell.Color &&
                !visited[current.Y][current.X - 1])
            {
                queue.Enqueue(grid[current.Y][current.X - 1]);
            }
        }
        
        return relevantCells;
    }
    
    // after there are holes in a grid, calculate how much each grid cell will have to move to fill
    // the holes beneath them (note that the grid cells won't actually be moving, they will just be swapping colors with the existing holes)
    private int[][] CalculateHolesBelowCells(GameGridCell[][] grid)
    {
        // reset the holes below cells array
        for (int y = 0; y < gridLength; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                holesBelowCells[y][x] = 0;
            } 
        }
        
        //bottom most row cannot fall, all entries will be 0, regardless of whether it is a hole or not
        //each row will collect amount of holes beneath it, and let the upper rows know
        for (int y = 1; y < gridLength; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                holesBelowCells[y][x] = grid[y - 1][x].Occupied ? holesBelowCells[y - 1][x] : holesBelowCells[y - 1][x] + 1;
            } 
        }

        return holesBelowCells;
    }
    
    //collect all grid cells where the corresponding holesBelowCells entry is more than 0 and the cell itself is not a hole
    //this indicates that those cells will fill existing cells
    private List<GameGridCell> CollectGridCellsThatFillHoles(GameGridCell[][] grid)
    {
        List<GameGridCell> cellsThatFillHoles = new List<GameGridCell>();
        for (int y = 1; y < gridLength; y++) // exclude bottom most row (non-empty cells cannot fill anything)
        {
            for (int x = 0; x < gridLength; x++)
            {
                if (!grid[y][x].Occupied) // if a cell is a hole itself, it cannot fill other holes
                {
                    continue;
                }
                if (holesBelowCells[y][x] > 0)
                {
                    cellsThatFillHoles.Add(grid[y][x]);
                }
            }
        }

        return cellsThatFillHoles;
    }
    
    //swap occupancy and colors of cells that fill holes, with the holes they will be filling
    private void SwapHolesWithExistingCells(List<GameGridCell> cellsThatFillHoles, GameGridCell[][] grid)
    {
        foreach (GameGridCell upperCell in cellsThatFillHoles)
        {
            int delta = holesBelowCells[upperCell.Y][upperCell.X];
            GameGridCell hole = grid[upperCell.Y - delta][upperCell.X];

            hole.Occupied = true;
            hole.Color = upperCell.Color;

            upperCell.Occupied = false;
            upperCell.Color = GameGridCell.GridCellColor.None;
        }
    }
    
    //populate the refill grid. positions where the main grid has holes will be populated in the refill grid
    private void PopulateRefillGrid(GameGridCell[][] source)
    {
        // first reset the whole refill grid
        for (int y = 0; y < gridLength; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                if (source[y][x].Occupied) // occupied cells in the main grid are empty in the refill grid
                {
                    refillGrid[y][x].Color = GameGridCell.GridCellColor.None;
                    refillGrid[y][x].Occupied = false;
                }
                else // empty cells in the main grid will be occupied in the refill grid
                {
                    refillGrid[y][x].Occupied = true;
                    refillGrid[y][x].Color = GetRandomGridCellColor(validColorCount);
                }
            } 
        }
    }
    
    // refill grid will have cells at exactly the points where the main grid will have holes
    // (and vice versa) complete the main grid by replacing hole cells with those from the refill grid
    private void CompleteMainGridUsingRefillGrid()
    {
        for (int y = 0; y < gridLength; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                if (mainGrid[y][x].Occupied)
                {
                    continue;
                }

                mainGrid[y][x].Occupied = true;
                mainGrid[y][x].Color = refillGrid[y][x].Color;
            } 
        }
    }
    
    // shuffle the entire grid
    private void ShuffleGameGrid(GameGridCell[][] grid)
    {
        //1. collect the colors of all the cells in the given grid
        List<GameGridCell.GridCellColor> cellColorList = new List<GameGridCell.GridCellColor>();
        for (int y = 0; y < gridLength; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                cellColorList.Add(grid[y][x].Color);
            }
        }
        
        //2. re-assign them randomly
        for (int y = 0; y < gridLength; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                int randomRoll = Random.Range(0, cellColorList.Count);
                grid[y][x].Color = cellColorList[randomRoll];
                cellColorList.RemoveAt(randomRoll);
            }
        }
        
        //3. Let other systems know that the grid was shuffled
        GameEvents.RaiseGridShuffledEvent(mainGrid);
    }
    
    // helper function to print a given grid to the console
    private void PrintGridToConsole(GameGridCell[][] grid)
    {
        string gridString = "";
        //swap from bottom first grid to top first console output
        for (int y = gridLength - 1; y >= 0; y --)
        {
            gridString += $"\n";
            for (int x = 0; x < gridLength; x++)
            {
                gridString += $" {GetGridCellStringFromColor(grid[y][x].Color)}";
            }
        }
        Debug.Log(gridString);
    }
    
    //helper to select color based on string input
    private string GetGridCellStringFromColor(GameGridCell.GridCellColor color)
    {
        switch (color)
        {
            case GameGridCell.GridCellColor.Red:
                return "R";

            case GameGridCell.GridCellColor.Green:
                return "G";

            case GameGridCell.GridCellColor.Blue:
                return "B";

            case GameGridCell.GridCellColor.Yellow:
                return "Y";
            
            case GameGridCell.GridCellColor.Orange:
                return "O";

            case GameGridCell.GridCellColor.Violet:
                return "V";
        }
        return "_";
    }
}
