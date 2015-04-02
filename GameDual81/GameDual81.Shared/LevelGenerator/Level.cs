using ThielynGame.GamePlay;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ThielynGame.LevelGenerator
{
    public enum LevelThemes {Dungeonm, Cave }

    public struct MazeNode 
    {
        public bool verticalWall, sideWall;
        public int group;
    }

    class Level
    {
        // Randomizer used to generate levels
        Random random;

        // this list contains all the section the current level consists of
        List<LevelSection> levelSections = new List<LevelSection>();

        // this list is only used during the level generation as a pool to randomly draw
        // sections from
        List<LevelSection> sectionPoolTopOpen; 
        List<LevelSection> sectionPoolSideWall;
        List<LevelSection> sectionPoolStandard;

        List<string> leveltextfiles_TopOpen = new List<string>();
        List<string> leveltextFiles_SideWall = new List<string>();
        List<string> levelTextFiles_standard = new List<string>();


        public void ActivateLevel(ObjectManager O) 
        {
            foreach (LevelSection L in levelSections) 
            {
                O.AddlevelTerrain(L.sectionTerrain);
            }
        }

        public void GenerateLevel(int levelWidth, int levelHeight, int difficulty, string TODOtextures) 
        {
            // create a list of files to use in level generation
            createFileList();
            // initialize a new random
            random = new Random();

            // re-initialize every time a new level is created
            levelSections = new List<LevelSection>();
            
            List<MazeNode> MazeMap = new List<MazeNode>();
            createMazeLayout(levelWidth, MazeMap);

            int mapIndex = 0;
            // fill in the level sections row by row, important LOOP START FROM TOP ROW
            // ROW 0 = top, Column 0 = left edge, 
            for (int row = 0; row < levelHeight; row++) 
            {
                for (int column = 0; column < levelWidth; column++) 
                {
                    createLevelSectionPool();
                    List<LevelSection> randomPool = new List<LevelSection>();

                    if (!MazeMap[mapIndex].verticalWall)
                    {
                        if (MazeMap[mapIndex].sideWall)
                            randomPool.AddRange(sectionPoolSideWall);
                        else
                            randomPool.AddRange(sectionPoolTopOpen);
                    }
                    else
                        randomPool.AddRange(sectionPoolStandard);

                    int rand = random.Next(1,randomPool.Count);

                    randomPool[rand - 1].AdjustPositions(row,column);
                    levelSections.Add( randomPool[rand - 1] );
                    mapIndex++;
                }
            }
        }

        void createMazeLayout(int width, List<MazeNode> mazeMapStorage) 
        {            
            MazeNode[] firstRow = new MazeNode[width];
            MazeNode[] secondRow = new MazeNode[width];

            random = new Random();

            // Randomizer flags and counters
            int lastSideWall = 0;

            // create upper row
            for (int x = 0; x < width; x++) 
            {
                MazeNode node = new MazeNode();
                node.verticalWall = true;

                if (random.Next(2, 5) < lastSideWall && x < width - 2 )
                {
                    node.sideWall = true;        
                    lastSideWall = 0;
                }
                else
                    lastSideWall++;
                // add the node to both lists
                firstRow[x] = node;
                secondRow[x] = node;
            }
            
            for (int x = 0; x < width; x++) 
            {
                if (secondRow[x].sideWall) 
                {
                    int distance = 0;
                    int whatToDo = random.Next(1,4);
 
                    switch (whatToDo)
                    {
                        case 1: distance = - 1; break;
                        case 2: distance = 1;   break;
                        case 3: distance = - 2; break;
                        case 4: distance = 2; break;
                    }

                    secondRow[x].sideWall = false;
                    secondRow[x + distance].sideWall = true;
                }
            }

            // set the vertical gaps
            for (int x = 0; x < width; x++)
            {
                if (firstRow[x].sideWall)
                    firstRow[x - 1].verticalWall = false;

                if (secondRow[x].sideWall) 
                    secondRow[x - 1].verticalWall = false;
            }

            // store nodes in the list provided by function caller
            mazeMapStorage.AddRange(firstRow);
            mazeMapStorage.AddRange(secondRow);
        }

        // this funtion instantiates level section objects from the levelfile.txt
        // data
        void createLevelSectionPool() 
        {
            sectionPoolStandard = new List<LevelSection>();
            sectionPoolSideWall = new List<LevelSection>();
            sectionPoolTopOpen = new List<LevelSection>();

            foreach (string s in levelTextFiles_standard) 
            {
                LevelSection l = new LevelSection();
                l.sectionTerrain = LevelFileReader.ReadTileInfo(s);

                sectionPoolStandard.Add(l);
            }

            foreach (string s in leveltextFiles_SideWall)
            {
                LevelSection l = new LevelSection();
                l.sectionTerrain = LevelFileReader.ReadTileInfo(s);

                sectionPoolSideWall.Add(l);
            }

            foreach (string s in leveltextfiles_TopOpen)
            {
                LevelSection l = new LevelSection();
                l.sectionTerrain = LevelFileReader.ReadTileInfo(s);

                sectionPoolTopOpen.Add(l);
            }
        }

        #region create list of files
        // have to do this shit manually....
        void createFileList() 
        {
            levelTextFiles_standard.Add("standard1.txt");
            levelTextFiles_standard.Add("standard2.txt");

            leveltextFiles_SideWall.Add("side_wall1.txt");
            leveltextFiles_SideWall.Add("side_wall2.txt");

            leveltextfiles_TopOpen.Add("top_open1.txt");
            leveltextfiles_TopOpen.Add("top_open2.txt");
            
        }
        #endregion
    }

}
