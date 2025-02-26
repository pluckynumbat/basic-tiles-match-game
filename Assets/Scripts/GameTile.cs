using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single tile in the game board
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class GameTile : MonoBehaviour
{
    private const float TILE_WIDTH = 1.0f;

    public int GridY; // Y index of this tile in the grid
    public int GridX; // X index of this tile in the grid

    public bool IsTileActive; // is this tile part of the active / main grid?

    private SpriteRenderer spriteRenderer;
    
    // limits that the tile cares about for input detection
    private float lowerX;
    private float upperX;
    private float lowerY;
    private float upperY;
    
    public void SetSprite(Sprite inputSprite)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = inputSprite;
    }

    // TODO: these will overlap at borders, fix that
    public void SetLimits(Vector2 position)
    {
        float halfWidth = TILE_WIDTH * 0.5f;
        lowerX = position.x - halfWidth;
        upperX = position.x + halfWidth;
        
        lowerY = position.y - halfWidth;
        upperY = position.y + halfWidth;
    }
}
