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
    public float refillContainerOffset; // where should the refill container be placed w.r.t. the tile container
    public Sprite[] tileSpriteOptions; // the different sprite options for the tiles
    public float tileFillHoleSpeed; // speed of a tile which is already on the main grid, falling to fill a hole
    public float newTileDropSpeed; // speed of a new tile dropping from above into the main grid during the last part of the player's move
    
    private readonly Vector2 offScreen = new Vector2(-1000, -1000);  // this is where the inactive tiles (and purgatory container) reside
    
    private Transform tileContainer; // parent gameObject for active tiles (part of the main grid)
    private Transform purgatoryContainer; // parent gameObject for deactivated tiles
    private Transform refillContainer; // parent gameObject for tiles that will refill the main grid during the last part of the player's move
    
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
        
        GameEvents.GridCellsFillHolesEvent -= OnGridCellsFillHoles;
        GameEvents.GridCellsFillHolesEvent += OnGridCellsFillHoles;
        
        GameEvents.RefillGridReadyEvent -= OnRefillGridReady;
        GameEvents.RefillGridReadyEvent += OnRefillGridReady;
    }
    private void OnDestroy()
    {
        GameEvents.GameGridReadyEvent -= OnGameGridReady;
        GameEvents.InputDetectedEvent -= OnInputDetected;
        GameEvents.InvalidMoveEvent -= OnInvalidMove;
        GameEvents.GridCellsRemovedEvent -= OnGridCellsRemoved;
        GameEvents.GridCellsFillHolesEvent -= OnGridCellsFillHoles;
        GameEvents.RefillGridReadyEvent -= OnRefillGridReady;
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
                activeTilesDictionary[(y * gridLength) + x] = newTile; 
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
        
        if (refillContainer == null)
        {
            refillContainer = new GameObject("RefillContainer").transform;
            refillContainer.transform.position = new Vector2(tileContainerPosition.x, tileContainerPosition.y + refillContainerOffset);
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
    private Sprite GetSpriteBasedOnColor(GameGridCell.GridCellColor color)
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
                Debug.LogError($"tile to deactivate is null! x:{cell.X}, y:{cell.Y}");
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
    
    //existing grid cells have filled holes in the grid,
    //assign new grid locations to the tiles representing them,
    //and then move these tiles to their new positions
    private void OnGridCellsFillHoles(List<GameGridCell> cellsThatFillHoles, int[][] holeDistances)
    {
        foreach (GameGridCell cell in cellsThatFillHoles)
        {
            // Part 1: changing the Y grid index of the tile and the entry of the tile in the active tiles dictionary
            
            // use the original key, and get the tile from it
            int originalKey = (cell.Y * gridLength) + cell.X;
            GameTile tileToMove = activeTilesDictionary[originalKey];
            
            // calculate the new grid Y index for the tile 
            int delta = holeDistances[cell.Y][cell.X];
            int newY = cell.Y - delta;
            
            // change the grid Y index associated with the tile
            tileToMove.GridY = newY;
            
            // calculate the new key for the tile
            int newKey =  (newY * gridLength) + cell.X;
            if (activeTilesDictionary[newKey] != null)  //this place should be vacant
            {
                Debug.LogError($"position to move into is not vacant! x:{cell.X}, y:{newY}");
                continue;
            }
            
            // move the tile to the new symbolic position in the dictionary
            activeTilesDictionary[newKey] = tileToMove;
            activeTilesDictionary[originalKey] = null;
            
            
            //Part 2. Actually move the tile
            
            // get new world position of the tile
            Vector2 newWorldPosition = GetWorldPositionFromGridPositionAndContainer(tileToMove.GridY, tileToMove.GridX, tileContainer);
            
            // set limits of the tile based on new world position
            tileToMove.SetLimits(newWorldPosition);
            
            // call the function to actually move the tile till it is in the correct spot
            StartCoroutine(tileToMove.MoveTileToNewPosition(newWorldPosition, tileFillHoleSpeed));
        }
    }
    
    //holes in the main tiles array now need to be filled at the end of the turn
    //new set of tiles will be put in the refill container
    //their starting positions informed by the holes distances in the refill grid (for visuals to start on same line)
    //final destinations of the tiles will be informed by their Y & X grid indices in the refill grid 
    private void OnRefillGridReady(GameGridCell[][] refillGrid, int[][] holeDistances)
    {
        List<GameTile> revivedTiles = new List<GameTile>();

        //1. first initialize tiles for all the new holes to be filled at the end of a turn
        for (int y = 0; y < gridLength; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                if (!refillGrid[y][x].Occupied)
                {
                    continue;
                }

                // bring the element from the front of the inactive tile pool queue
                GameTile revivedTile = inactiveTilesQueue.Dequeue();

                // set it up like a new tile, but it goes in the refill container first
                SetupGameTile(revivedTile, y, x, refillGrid[y][x], refillContainer);

                // add tile to active tile dictionary
                activeTilesDictionary[(y * gridLength) + x] = revivedTile;

                //activate the tile's sprite renderer
                revivedTile.SetSpriteVisibility(true);

                //add it to the list of newly revived tiles
                revivedTiles.Add(revivedTile);
            }
        }
        
        //2. add adjustment based on the hole distances in the refill grid (for visual purposes)
        // this is done so that all the incoming tiles from the refill container
        // are aligned at the bottom row's Y value instead of the top row's Y value
        ////////// for example ////////////
        //   before:                after:
        //   TTTTT                  T---T
        //   TT-TT                  TT-TT
        //   T---T                  TTTTT
        /////////////////////////////////
        foreach (GameTile refillTile in revivedTiles)
        {
            //move the tiles to the bottom of their current container (so that all holes are above)
            if (holeDistances[refillTile.GridY][refillTile.GridX] > 0)
            {
                float adjustedY = refillTile.transform.localPosition.y - holeDistances[refillTile.GridY][refillTile.GridX];
                refillTile.transform.localPosition = new Vector2(refillTile.transform.localPosition.x, adjustedY);
            }
        }
        
        //3. let the new tiles come in and take their place in the main tile container
        // also calculate required time for last tile to reach its position, that's when we can re-enable input
        float maxDistance = 0f;
        foreach (GameTile refillTile in revivedTiles)
        {
            //set parent to the tile container
            refillTile.transform.SetParent(tileContainer);
            
            //get new world position of the tile
            Vector2 newWorldPosition = GetWorldPositionFromGridPositionAndContainer(refillTile.GridY, refillTile.GridX, tileContainer);
            
            //set limits of the tile based on new world position
            refillTile.SetLimits(newWorldPosition);
            
            //check the distance of its upcoming move vs max distance till now
            if (refillTile.transform.position.y - newWorldPosition.y > maxDistance)
            {
                maxDistance = refillTile.transform.position.y - newWorldPosition.y;
            }
            
            //call the function to actually move the tile till it is in the correct spot
            StartCoroutine(refillTile.MoveTileToNewPosition(newWorldPosition, newTileDropSpeed));
        }
    }
}
