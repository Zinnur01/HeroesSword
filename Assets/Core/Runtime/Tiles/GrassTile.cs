public class GrassTile : Tile
{
    public override bool IsWalkable()
    {
        return GetOccupiedUnit() == null;
    }
}
