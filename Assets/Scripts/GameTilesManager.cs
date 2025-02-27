using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the functionality related to creating and modifying the game tiles
/// which are the View part of the game board
/// </summary>
public class GameTilesManager : MonoBehaviour
{
    public GameObject tilePrefab; // prefab for the tiles
    public Vector2 tileContainerPosition; // where should the tile container be placed
    public Sprite[] tileSpriteOptions;
    
    private readonly Vector2 offScreen = new Vector2(-1000, -1000);
    
    private Transform tileContainer; // parent gameObject for active tiles (part of the main grid)
    private Transform purgatoryContainer; // parent gameObject for deactivated tiles
    
    private Dictionary<int, GameTile> activeTilesDictionary; // collection of all the active tiles (part of the main grid)
    private Queue<GameTile> inactiveTilesQueue; // a collection (pool) of currently inactive tiles (that can be re-used)
    
    private int gridLength; // cache the grid length in this
    
    private bool acceptingInput = false; // can the player interact with the tiles on the game board?
    
    private void Awake()
    {
        GameEvents.GameGridReadyEvent -= OnGameGridReady;
        GameEvents.GameGridReadyEvent += OnGameGridReady;
        
        GameEvents.InputDetectedEvent -= OnInputDetected;
        GameEvents.InputDetectedEvent += OnInputDetected;
        
        GameEvents.InvalidMoveEvent -= OnInvalidMove;
        GameEvents.InvalidMoveEvent += OnInvalidMove;
        
        GameEvents.GridCellsRemovedEvent -= OnGridCellsRemoved;
        GameEvents.GridCellsRemovedEvent += OnGridCellsRemoved;
    }
    private void OnDestroy()
    {
        GameEvents.GameGridReadyEvent -= OnGameGridReady;
        GameEvents.InputDetectedEvent -= OnInputDetected;
        GameEvents.InvalidMoveEvent -= OnInvalidMove;
        GameEvents.GridCellsRemovedEvent -= OnGridCellsRemoved;
    }

    // main grid is ready at the beginning of a level, create tiles 
    private void OnGameGridReady(GameGridCell[][] gameGrid)
    {
        CreateTiles(gameGrid);
        acceptingInput = true;
    }
    
    // main grid is ready at the beginning of a level, create tiles 
    private void CreateTiles(GameGridCell[][] gameGrid)
    {
        // 1. create other required structures
        CreateTileContainers();
        CreateTileCollections();
        
        // 2. cache the grid length
        gridLength = gameGrid.Length;

        // 3. instantiate GameTile GameObjects for each active tile 
        for (int y = 0; y < gridLength; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                GameTile newTile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity).GetComponent<GameTile>();
               
                // setup various properties of the game tile
                SetupGameTile(newTile, y, x, gameGrid[y][x], tileContainer);
                                
                // add tile to active tiles dictionary at a key created from the tile's game grid Y and X indices
                activeTilesDictionary[(y * gameGrid.Length) + x] = newTile; 
            }
        }
        
        //4. create a buffer of inactive tiles
        for (int i = 0;  i < gridLength * gridLength; i++)
        {
            GameTile tile = Instantiate(tilePrefab, offScreen, Quaternion.identity).GetComponent<GameTile>();
            DeactivateTile(tile);
        }
    }
    
    private void CreateTileContainers()
    {
        if (tileContainer == null)
        {
            tileContainer = new GameObject("TileContainer").transform;
            tileContainer.transform.position = tileContainerPosition;
        }
        
        if (purgatoryContainer == null)
        {
            purgatoryContainer = new GameObject("PurgatoryContainer").transform;
            purgatoryContainer.transform.position = offScreen;
        }
    }
    
    private void CreateTileCollections()
    {
        if (activeTilesDictionary == null)
        {
            activeTilesDictionary = new Dictionary<int, GameTile>();
        }
        
        if (inactiveTilesQueue == null)
        {
            inactiveTilesQueue = new Queue<GameTile>();
        }
    }
    
    // assign key properties to the game tile, based on the game grid
    private void SetupGameTile(GameTile tile, int gridY, int gridX, GameGridCell cell, Transform container)
    {
        //set properties
        tile.GridY = gridY;
        tile.GridX = gridX;
        tile.SetSprite(GetSpriteBasedOnColor(cell.Color));
                
        //set the input container as the tile's parent
        tile.transform.SetParent(container);
                
        //set position based on the input container
        tile.transform.position = GetWorldPositionFromGridPositionAndContainer(tile.GridY, tile.GridX, container);
                
        // set limits that the tile should care about based on the grid item it will represent
        tile.SetLimits(tile.transform.position);
        
        //set the tile as active
        tile.IsTileActive = true;
    }
    
    // provides mapping from a tile's Y & X indices in the game grid to its position in the world, based on the container it is a part of
    private Vector2 GetWorldPositionFromGridPositionAndContainer(int gridY, int gridX, Transform container)
    {
        // grids go from 0 to gridLength, tile containers are centered on the X axis
        float mappingY = gridY - (gridLength * 0.5f) + 0.5f;
        float mappingX = gridX - (gridLength * 0.5f) + 0.5f;
        return new Vector2(container.position.x + (mappingX), container.position.y + (mappingY));
    }
    
    // helper function to select the sprite from the available options based on the input color
    public Sprite GetSpriteBasedOnColor(GameGridCell.GridCellColor color)
    {
        switch (color)
        {
            case GameGridCell.GridCellColor.Red:
                return tileSpriteOptions[0];
            
            case GameGridCell.GridCellColor.Green:
                return tileSpriteOptions[1];
            
            case GameGridCell.GridCellColor.Blue:
                return tileSpriteOptions[2];
            
            case GameGridCell.GridCellColor.Yellow:
                return tileSpriteOptions[3];
        }

        return null;
    }
    
    // check if the player attempted a move on the game board
    private void OnInputDetected(Vector3 inputWorldPosition)
    {
        if (!acceptingInput)
        {
            return;
        }

        GameTile activeTileTapped = null;
        foreach (GameTile tile in activeTilesDictionary.Values)
        {
            if (tile == null || !tile.IsTileActive)
            {
                continue;
            }
            
            if (tile.IsPositionWithinMyLimits(inputWorldPosition))
            {
                // player tapped an active tile!
                activeTileTapped = tile;
                break; // only care for one input at a time
            }
        }

        // send the tile's grid Y & X indices for further processing
        if (activeTileTapped != null)
        {
            acceptingInput = false;
            GameEvents.RaiseActiveTileTappedEvent(activeTileTapped.GridY, activeTileTapped.GridX);
        }
    }
    
    // the player's attempted move was invalid
    private void OnInvalidMove(int gridY, int gridX)
    {
        //TODO: check if it is safe to set this to true in all cases
        acceptingInput = true;
    }
    
    // some grid cells have been removed from the game grid, so remove the corresponding game tiles from the tiles container
    // and de-activate them
    private void OnGridCellsRemoved(List<GameGridCell> removedCells)
    {
        foreach (GameGridCell cell in removedCells)
        {
            int key = (cell.Y * gridLength) + cell.X; // key into the dictionary based on Y & X indices
            GameTile tileToDeactivate = activeTilesDictionary[key];
            
            //this tile should not be null
            if (tileToDeactivate == null)
            {
                Debug.LogError($"tile to deactivate is null! x:{cell.Y}, y:{cell.X}");
                continue;
            }
            
            DeactivateTile(tileToDeactivate);
            activeTilesDictionary[key] = null; // remove the entry from the active tiles dictionary
        }
    }
    
    // De-activate a given game tile (disable its sprite, send it to the purgatory container, and add it to the pool of inactive tiles)
    private void DeactivateTile(GameTile tileToDeactivate)
    {
        tileToDeactivate.IsTileActive = false;
        tileToDeactivate.SetSpriteVisibility(false);
        tileToDeactivate.transform.SetParent(purgatoryContainer);
        tileToDeactivate.transform.position = offScreen;
        inactiveTilesQueue.Enqueue(tileToDeactivate);
    }
}
