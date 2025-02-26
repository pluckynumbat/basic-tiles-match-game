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
    
    private Transform tileContainer; // parent gameObject for active tiles (part of the main grid)
    
    private Dictionary<int, GameTile> activeTilesDictionary; // collection of all the active tiles (part of the main grid)
    
    private int gridLength; // cache the grid length in this
    
    private void Awake()
    {
        GameEvents.GameGridReadyEvent -= OnGameGridReady;
        GameEvents.GameGridReadyEvent += OnGameGridReady;
    }
    private void OnDestroy()
    {
        GameEvents.GameGridReadyEvent -= OnGameGridReady;
    }

    // main grid is ready at the beginning of a level, create tiles 
    private void OnGameGridReady(GameGridCell[][] gameGrid)
    {
        CreateTiles(gameGrid);
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
    }
    
    private void CreateTileContainers()
    {
        if (tileContainer == null)
        {
            tileContainer = new GameObject("TileContainer").transform;
            tileContainer.transform.position = tileContainerPosition;
        }
    }
    
    private void CreateTileCollections()
    {
        if (activeTilesDictionary == null)
        {
            activeTilesDictionary = new Dictionary<int, GameTile>();
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
}
