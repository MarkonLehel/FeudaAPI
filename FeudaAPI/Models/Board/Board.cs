using FeudaAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models.Board
{
    public class Board
    {
        public Board()
        {
            GenerateRandomBoard();
        }

        private int _desiredForests = 6;
        private int _desiredMountains = 6;
        private int _desiredFields = 12;
        public Tile[,] BoardTiles { get; } = new Tile[5, 5];


        public void MoveSerf(Coordinate from, Coordinate to)
        {
            Tile fromTile = GetTile(from);
            Tile toTile = GetTile(to);
            if (fromTile.HasSerf && !toTile.HasSerf)
            {
                fromTile.HasSerf = false;
                toTile.HasSerf = true;
            }
        }


        private Tile GetTile(int x, int y)
        {
            return BoardTiles[y, x];
        }

        private Tile GetTile(Coordinate cord)
        {
            return GetTile(cord.x, cord.y);
        }

        //This can be done better
        private void GenerateRandomBoard()
        {
            int _currentForests = 0;
            int _currentMountains = 0;
            int _currentFields = 0;

            List<TileType> validTiles = new() { TileType.Field, TileType.Mountain, TileType.Forest };

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {

                    if (_currentForests == _desiredForests && validTiles.Contains(TileType.Forest))
                        validTiles.Remove(TileType.Forest);
                    if (_currentFields == _desiredFields && validTiles.Contains(TileType.Field))
                        validTiles.Remove(TileType.Field);
                    if (_currentMountains == _desiredMountains && validTiles.Contains(TileType.Mountain))
                        validTiles.Remove(TileType.Mountain);

                    bool _gotAvailableType = false;

                    while (!_gotAvailableType)
                    {

                    }

                }

            }







        }
    }
}
