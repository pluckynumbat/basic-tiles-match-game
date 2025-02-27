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

    private bool isTileMoving;
    
    public void SetSprite(Sprite inputSprite)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = inputSprite;
    }
    
    public void SetSpriteVisibility(bool visible)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.enabled = visible;
    }

    // TODO: these will overlap at borders, fix that
    // these are the positions the tile cares about when detecting input
    public void SetLimits(Vector2 position)
    {
        float halfWidth = TILE_WIDTH * 0.5f;
        lowerX = position.x - halfWidth;
        upperX = position.x + halfWidth;
        
        lowerY = position.y - halfWidth;
        upperY = position.y + halfWidth;
    }
    
    //used to check if player interacted with this tile
    public bool IsPositionWithinMyLimits(Vector2 inputPosition)
    {
        return lowerX < inputPosition.x && inputPosition.x < upperX && 
               lowerY < inputPosition.y && inputPosition.y < upperY;
    }
    
    // drop the tile into its new position
    public IEnumerator MoveTileToNewPosition(Vector2 newPosition, float tileSpeed)
    {
        if (isTileMoving)
        {
            Debug.LogError("tile is already moving, abort");
            yield break;
        }
        
        isTileMoving = true;
        
        // we will only be changing the Y position
        float initialYValue = transform.position.y;
        float finalYValue = newPosition.y;
        
        // calculate fall time
        float yDistance = transform.position.y - newPosition.y;
        float fallTime = yDistance / tileSpeed;

        //perform a linear interpolation of the Y value over fall time
        float timeSinceStart = 0f;
        while (timeSinceStart < fallTime)
        {
            float yValue = Mathf.Lerp(initialYValue, finalYValue, timeSinceStart / fallTime);
            transform.position = new Vector2(transform.position.x, yValue);
            timeSinceStart += Time.deltaTime;
            yield return null;
        }

        // set final position
        transform.position = newPosition;
        isTileMoving = false;
    }
}
