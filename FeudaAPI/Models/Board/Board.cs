using FeudaAPI.Models.DataHolder;
using System.Collections.Generic;

namespace FeudaAPI.Models
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
        public List<Tile> TilesWithSerfs = new();


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


        public Tile GetTile(int x, int y)
        {
            return BoardTiles[y, x];
        }

        public Tile GetTile(Coordinate cord)
        {
            return GetTile(cord.x, cord.y);
        }

        
        private void GenerateRandomBoard()
        {
            int _currentForests = 0;
            int _currentMountains = 0;
            int _currentFields = 0;

            //Set up town tile
            BoardTiles[2, 2] = new Tile(new Coordinate(2, 2), TileType.Town);
            Tile townTile = GetTile(2, 2);
            townTile.BaseTileIncome = 0;
            townTile.Building = Data.GetBuildingDataForType(BuildingType.Town);
            townTile.HasBuilding = true;
            TilesWithSerfs.Add(townTile);

            //Set up tile types
            List<TileType> validTiles = new() { TileType.Field, TileType.Mountain, TileType.Forest };

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {

                    if (_currentForests == _desiredForests && validTiles.Contains(TileType.Forest))
                        validTiles.Remove(TileType.Forest);
                    if (_currentFields == _desiredFields && validTiles.Contains(TileType.Field))
                        validTiles.Remove(TileType.Field);
                    if (_currentMountains == _desiredMountains && validTiles.Contains(TileType.Mountain))
                        validTiles.Remove(TileType.Mountain);

                    bool _gotAvailableType = false;

                    Tile tile = new Tile(new Coordinate(x, y), null);
                    BoardTiles[y, x] = tile;
                    
                    while (!_gotAvailableType)
                    {
                        if (tile.TileType == null)
                        {
                            tile.TileType = validTiles[Data.random.Next(validTiles.Count)];
                            _gotAvailableType = true;
                        }
                    }

                }

            }







        }
    }
}
