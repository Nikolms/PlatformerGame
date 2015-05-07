using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay;
using ThielynGame.LevelGenerator;

namespace GameDual81.LevelGenerator
{
    class LevelGeneratorObject
    {
        List<LevelSection> levelsections;
        Random random;



        // generate randomized combination of level sections when instantiated
        public LevelGeneratorObject(int levelSize) 
        {
            random = new Random();
            levelsections = new List<LevelSection>();

            // create as many random sections as the levelsize
            for (int x = 0; x < levelSize; x++) 
            {
                createRandomSection(x);
            }
        }




        // creates a random number and chooses item on list based on that, (need better system)
        void createRandomSection (int iterationCount) 
        {
            int randomValue = random.Next(1,4);
            LevelSection section = new LevelSection();

            #region too long switch :(

            // add new cases on top
            switch (randomValue) 
            {
                case 4:

                case 3:
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 576, 256, 192)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(356, 576, 410, 192)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(866, 676, 100, 92)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(1066, 576, 214, 192)));
                    break;

                case 2:
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 512, 256, 256)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(356, 400, 568, 150)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(1024, 512, 256, 256)));
                    break;

                case 1:
                    section.sectionTerrain.Add(new Platform(new Rectangle(0, 512, 640, 256)));
                    section.sectionTerrain.Add(new Platform(new Rectangle(640, 512, 640, 256)));
                    break;
            }

            #endregion

            // call the method to adjust terrain piece positions based on the position of the section itself
            section.AdjustPositions(0,iterationCount);

            levelsections.Add(section);
        }




        // takes a List instance and fills/adds it with level Objects
        public void getLevelInfo(List<GameObject> list) 
        {
            foreach (LevelSection L in levelsections) 
            {
                list.AddRange(L.sectionTerrain);
            }
        }
    }

    
}
