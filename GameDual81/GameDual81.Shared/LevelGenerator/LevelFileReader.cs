using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ThielynGame.GamePlay;

namespace ThielynGame.LevelGenerator
{
    public class LevelFileReader
    {
        // takes a filename to read, and constructs a list of tiles
        // to return
        public static List<Platform> ReadTileInfo(string fileName) 
        {
            List<Platform> tiles = new List<Platform>();

           using (StreamReader sr = 
               new StreamReader(TitleContainer.OpenStream(
                   "Content/Levels/" + fileName))) 
           {
               //There are always 24*20 tiles on are map
               // including empty grids
               for (int x = 0; x < 20; x++) 
               {
                   for (int y = 0; y < 24; y++) 
                   {
                       string code = sr.ReadLine();

                       switch (code) 
                       {
                           case "EE": break;

                           case "MM":
                           tiles.Add(new Platform
                               (new Rectangle(x*64, y*32, 64,32),
                               TilePosition.Middle)); break;

                        case "TT":
                           tiles.Add(new Platform
                               (new Rectangle(x, y*32, 64,32),
                               TilePosition.Top)); break;

                        default: break;
                      }
                   }
               }

           }
            // returns all the tiles that were found in text file
           return tiles;
        }
    }
}
