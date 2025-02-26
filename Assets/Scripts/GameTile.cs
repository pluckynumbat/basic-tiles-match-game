using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single tile in the game board
/// </summary>
public class GameTile : MonoBehaviour
{
    private const float TILE_WIDTH = 1.0f;

    public int GridY; // Y index of this tile in the grid
    public int GridX; // X index of this tile in the grid

    public bool IsTileActive; // is this tile part of the active / main grid?
}
