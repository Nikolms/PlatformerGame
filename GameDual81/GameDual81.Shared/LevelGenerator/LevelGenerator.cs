using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay;
using ThielynGame.GamePlay.Traps;
using ThielynGame.LevelGenerator;

namespace GameDual81.LevelGenerator
{
    enum NodeType { Common, TopWall, BottomWall, TopOpen, BottomOpen, Exit}

    struct MazeNode 
    {
        public bool IsMainPath;
        public NodeType nodeType;
    }


    class LevelGeneratorObject
    {
        List<MazeNode> mazeMap;  
        List<LevelSection> levelsections;
        Random random;



        // generate randomized combination of level sections when instantiated
        public LevelGeneratorObject(int levelSize) 
        {
            random = new Random();
            levelsections = new List<LevelSection>();

            // create the maze layout
            createMazeLayout(levelSize);
            int indexAdjust = 0;

            // read the mazemap and fill with random sections
            for (int row = 0; row < 2; row++)
            {
                for (int column = 0; column < levelSize; column ++ )
                {
                    LevelSection section = new LevelSection();
                    NodeType M = mazeMap[column + indexAdjust].nodeType;

                    if (M == NodeType.TopOpen) randomTopOpen(section);
                    if (M == NodeType.TopWall) randomTopWall(section);
                    if (M == NodeType.BottomOpen) randomBottomOpen(section);
                    if (M == NodeType.BottomWall) randomBottomWall(section);
                    if (M == NodeType.Common) randomCommon(section);
                    if (M == NodeType.Exit) randomEnding(section);

                    section.AdjustPositions(row, column);
                    levelsections.Add(section);
                }
                indexAdjust = levelSize;
            }
        }



        // this function creates a list of maze nodes with random access points
        void createMazeLayout(int levelSize) 
        {
            MazeNode[] topRow = new MazeNode[levelSize];
            MazeNode[] bottomRow = new MazeNode[levelSize];

            int counter = 0;

            for (int x = 0; x < levelSize; x++ )
            {
                MazeNode M = new MazeNode {nodeType = NodeType.Common };

                counter++;

                if (random.Next(2, 5) < counter && x < levelSize - 3 )
                {
                   M.nodeType = NodeType.TopWall;

                    counter = 0;
                }

                topRow[x] = M;
                bottomRow[x] = M;
            }

            
            for (int x = 0; x< levelSize; x++)
            {
                if (topRow[x].nodeType == NodeType.TopWall) 
                {
                    bottomRow[x].nodeType = NodeType.BottomOpen;
                    
                    int r = random.Next(1,5);

                    switch (r) 
                    {
                        case 1:
                            bottomRow[x - 2].nodeType = NodeType.BottomWall;
                            topRow[x - 2].nodeType = NodeType.TopOpen;
                            break;

                        case 2: 
                            bottomRow[x - 1].nodeType = NodeType.BottomWall;
                            topRow[x - 1].nodeType = NodeType.TopOpen;
                            break;

                        case 3: 
                            bottomRow[x + 1].nodeType = NodeType.BottomWall;
                            topRow[x + 1].nodeType = NodeType.TopOpen;
                            break;

                        case 4: 
                            bottomRow[x + 2].nodeType = NodeType.BottomWall;
                            topRow[x + 2].nodeType = NodeType.TopOpen;
                            break;
                    }
                }
            }

            topRow[levelSize - 2].nodeType = NodeType.Exit;
            
            mazeMap = new List<MazeNode>();
            mazeMap.AddRange(topRow);
            mazeMap.AddRange(bottomRow);
        }



        // takes a List instance and fills/adds it with level Objects
        public void getLevelInfo(List<GameObject> list) 
        {
            foreach (LevelSection L in levelsections) 
            {
                list.AddRange(L.sectionTerrain);
            }
        }



        void randomTopWall(LevelSection section) 
        {
            int i = random.Next(1,2);
            switch (i) 
            {
                case 1: 
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 568, 410, 200)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(410, 698, 300, 70)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(1030, 0, 250, 768)));
                    break;
            }
        }

        void randomTopOpen(LevelSection section) 
        {
            int i = random.Next(1,2);

            switch (i) 
            {
                case 1: 
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 568, 410, 200)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(410, 698, 300, 70)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(1030, 700, 120, 95)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(1150, 620, 130, 175)));
                    break;
            }
        }

        void randomBottomWall(LevelSection section) 
        {
            int i = random.Next(1,2);
            switch (i) 
            {
                case 1:
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 518, 310, 250)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(310, 368, 200, 400)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(510, 218, 200, 550)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(710, 68, 320, 698)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(1030, 0, 250, 768)));
                    break;
            }
        }

        void randomBottomOpen(LevelSection section) 
        {
            int i = random.Next(1,2);

            switch (i) 
            {
                case 1:
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 518, 200, 250)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(200, 368, 200, 400)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(400, 618, 880, 150)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(500, 258, 780, 70)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(610, 158, 670, 100)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(710, 68, 570, 90)));
                    break;

            }
        }

        void randomCommon(LevelSection section) 
        {
            // increase random max value as new switch cases are added
            int i = random.Next(1,4);

            switch (i) 
            {
                case 3:
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 512, 256, 256)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(356, 450, 160, 40), "", new Vector2(3,0), 408));
                    section.sectionTerrain.Add(new Platform(new Rectangle(1024, 512, 256, 256)));
                    break;

                case 2: 
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 492, 640, 236)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(640, 512, 640, 256)));

                    section.sectionTerrain.Add(new Spikes(1, new Rectangle(500,100,60,60), new Vector2(0,3), 500));
                    section.sectionTerrain.Add(new Spikes(1, new Rectangle(800,100,60,60), new Vector2(0, 6), 500));
                    break;

                case 1: 
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 512, 256, 256)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(356, 400, 568, 150)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(1024, 512, 256, 256)));

                    section.sectionTerrain.Add(new Spikes(20,new Rectangle(420,362,100,40)));
                    break;
            }


        }

        void randomEnding(LevelSection section) 
        {
            int i = random.Next(1,2);

            switch (i) 
            {
                case 1:
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 543, 430, 225)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(430, 393, 420, 375)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(850, 543, 430, 225)));

                    section.sectionTerrain.Add(new Spikes(20,new Rectangle(330,513,100,32)));
                    section.sectionTerrain.Add(new Spikes(20, new Rectangle(850, 513, 100, 32)));
                    break;
            }
        }
    }

    
}
