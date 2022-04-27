
using FeudaAPI.Models;

namespace FeudaAPI.Logic
{
    public class BoardLogic
    {

        public static Tile GetTile(int x, int y, GameBoard board)
        {
            return board.BoardTiles[y, x];
        }

        public static Tile GetTile(Coordinate cord, GameBoard board)
        {
            return GetTile(cord.x, cord.y, board);
        }

        public static bool MoveSerf(Coordinate from, Coordinate to, GameBoard board)
        {
            Tile fromTile = GetTile(from, board);
            Tile toTile = GetTile(to, board);
            if (fromTile.HasSerf && !toTile.HasSerf)
            {
                fromTile.HasSerf = false;
                toTile.HasSerf = true;
                return true;
            }
            return false;
        }
    }
}
