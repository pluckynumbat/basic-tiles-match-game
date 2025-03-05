using UnityEngine;

/// <summary>
/// Represent a single cell in the game grid(s)
/// </summary>
public class GameGridCell
{
    // possible color options
    public enum GridCellColor
    {
        None = -1,
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Violet,
    }

    public readonly int Y; // y index of this cell in the grid it is a part of
    public readonly int X; // x index of this cell in the grid it is a part of

    public bool Occupied; // is this cell occupied?
    public GridCellColor Color; // what is the color of the occupying item (None if not occupied)

    public GameGridCell(int y, int x)
    {
        Y = y;
        X = x;
    }
    
    //helper to select color based on string input
    public static GridCellColor GetGridCellColorFromString(string colorString)
    {
        switch (colorString)
        {
            case "R":
                return GridCellColor.Red;
            
            case "G":
                return GridCellColor.Green;
            
            case "B":
                return GridCellColor.Blue;
            
            case "Y":
                return GridCellColor.Yellow;
            
            case "O":
                return GridCellColor.Orange;
            
            case "V":
                return GridCellColor.Violet;
        }
        
        Debug.LogError($"Invalid color string: {colorString}");
        return GridCellColor.None;
    }
    
    //helper to cell color string based on cell color input
    public static string GetGridCellStringFromColor(GridCellColor color)
    {
        switch (color)
        {
            case GridCellColor.Red:
                return "R";

            case GridCellColor.Green:
                return "G";

            case GridCellColor.Blue:
                return "B";

            case GridCellColor.Yellow:
                return "Y";
            
            case GridCellColor.Orange:
                return "O";

            case GridCellColor.Violet:
                return "V";
        }
        return "_";
    }
}
