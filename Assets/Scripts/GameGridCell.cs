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
}
