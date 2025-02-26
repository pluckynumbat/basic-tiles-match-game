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
    
}
