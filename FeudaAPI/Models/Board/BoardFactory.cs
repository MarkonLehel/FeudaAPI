using FeudaAPI.Models.Data;
using System.Collections.Generic;

namespace FeudaAPI.Models.Factories
{
    public class BoardFactory
    {

        private int _desiredForests = 6;
        private int _desiredMountains = 6;
        private int _desiredFields = 12;

        public static GameBoard GenerateRandomBoard(int desiredForests, int desiredMountains, int desiredFields)
        {

            GameBoard board = new GameBoard();
            int _currentForests = 0;
            int _currentMountains = 0;
            int _currentFields = 0;

            //Set up town tile
            board.BoardTiles[2, 2] = new Tile(new Coordinate(2, 2), TileType.Town);
            Tile townTile = board.BoardTiles[2, 2];
            townTile.BaseTileIncome = 0;
            townTile.Building = Data.Data.GetBuildingDataForType(BuildingType.Town);
            townTile.HasBuilding = true;
            board.TilesWithSerfs.Add(townTile);

            //Set up tile types
            List<TileType> validTiles = new() { TileType.Field, TileType.Mountain, TileType.Forest };

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {

                    if (_currentForests == desiredForests && validTiles.Contains(TileType.Forest))
                        validTiles.Remove(TileType.Forest);
                    if (_currentFields == desiredFields && validTiles.Contains(TileType.Field))
                        validTiles.Remove(TileType.Field);
                    if (_currentMountains == desiredMountains && validTiles.Contains(TileType.Mountain))
                        validTiles.Remove(TileType.Mountain);

                    bool _gotAvailableType = false;

                    Tile tile = new Tile(new Coordinate(x, y), null);
                    board.BoardTiles[y, x] = tile;

                    while (!_gotAvailableType)
                    {
                        if (tile.TileType == null)
                        {
                            tile.TileType = validTiles[Data.Data.random.Next(validTiles.Count)];
                            _gotAvailableType = true;
                        }
                    }

                }

            }
            return board;
        }
    }
}










//                                 d8888888888888888888888 "
//                                888888888888888888PYP "'
//                               d88888888888888888D
//                               8888888888888888P '
//                                Y8888888888888b
//                               C8888888Y888888P
//                                Y88888'd88888"      
//                                8888P d8888P 
//                               d8888D 88888   
//                              d888P'  Y88dP   
//                             nY88Pn    Y88            8"""-----....._____
//                             N   +N    88'            8                  NNNNNN8
//                             N   +N  nd88n            P                  NNNNNNP
//                             N   +N  N  +N           d  dNN   ...       dNNNNNN
//     __...---"Nn.            N   +N  N  +N           8  NNP  dNNP  dNN  NNNNNNN
//  8""         NNNNn          N   +N  N  +N           8       ""'   NNP  NNNNNNN
//  8       oo  NNNNN          N   +N  N  +N           8                  NNNNNNP
//  Y  dN   NN  NNNNN          N   +N  N  +N           P       ooo       dNNNNNN
//   b NY   ""  YNNNN          N   +N  N  +N          d       dNN'  dNN  NNNNNNN
//   8        _  bNNNb         N   +N  N  +N          8       """   NNP  NNNNNNN
//   8  o8   88  NNNNN         N   +N  N  +N          8                  NNNNNNN
//   Y  BP   ""  NNNNN         N   +N  N  +N          8            nnn   NNNNNNP
//    b          NNNNN         N   +N  N  +N          P            NNP  dNNNNNN
//    8          YNNNN         N   +N  M  +N         d             ""   NNNNNNN
//    8           NNNNb        N   +N  N  +N         8                  NNNNNNN
//    Y           NNNNN      __N___+N__N  +N         8                  NNNNNNP
//     b          NNNNNooooodP""""""""YNNNNNNbcgmmnnn8                 dNNNNNN
//     8          """'                         `"""""8                 NNNNNNN
//     8                        BOARD FACTORY        P                 NNNNNNN
//     Y                          NNNNNNNNN         d                  NNNNNNN
//      b                         NNNNNNNNN         8                  NNNNNNP
//      8                         NNNNNNNNP         8                 dNNNNNN
//      8                         NNNNNNNN; 8                 NNNNNNN
//Y                         NNNNNNNN:         P NNNNNNN
//       b                        NNNNNNNN; d NNNNNNP
//       8                        NNNNNNNN         8                 dNNNNNN 
// ______8__........----------""""""""""""------...8..______         NNNNNNN
//_________........----------""""""""""""------......_____  """""----NNNNNNN
//                                                         """""----....___ """--
//                                                                         """---
