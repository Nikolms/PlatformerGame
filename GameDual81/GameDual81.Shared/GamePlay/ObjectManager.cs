using ThielynGame.AnimationFiles;
using ThielynGame.LevelGenerator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDual81.LevelGenerator;

namespace ThielynGame.GamePlay
{
    class ObjectManager
    {  
        // stores all the gameobjects currently in the game
        static List <GameObject> masterlist = null;

        // this list is needed so that we can add gameobjects mid update loop
        // and not lose them before next frame when masterlist is overwritten
        static List<GameObject> NewObjectsWaitList = new List<GameObject>();

        List<GameObject> updateAndrenderList;

        // we need to keep track of player separately too
        Player player;

        public ObjectManager() { }
        public ObjectManager(Player player) 
        {
            if (masterlist == null)
                masterlist = new List<GameObject>();

            this.player = player;
        }

        public void StartLevel(int levelSize, int difficulty) 
        {
            // overwrite masterlist everytime a new level is to be created
            masterlist = new List<GameObject>();

            LevelGeneratorObject lg = new LevelGeneratorObject(levelSize);

            // inject masterlist to levelgenerator to get all the objects
            lg.getLevelInfo(masterlist);
        }

        public void Load() 
        {
        }

        //all game objects are updated in this method
        public void Update(TimeSpan time) 
        {
            // add any objects to masterlist
            masterlist.AddRange(NewObjectsWaitList);
            // clear the waitinglist
            NewObjectsWaitList.Clear();

            // a list that stores all objects that are not dead, 
            List<GameObject> AliveObjects = new List<GameObject>();
            // a list for all the terrain that matters this frame
            List<Platform> terrainToUpdate = new List<Platform>();
            List<MovableObject> movableObjectsToUpdate = new List<MovableObject>();
            List<ICollisionObject> interactiveObjects = new List<ICollisionObject>();
            List<Character> activeCharacters = new List<Character>();
            List<GameObject> miscObjects = new List<GameObject>();

            updateAndrenderList = new List<GameObject>();

            // this loop check the entire masterlist, sorts objects into categories 
            // that need updating and drops dead objects
            foreach (GameObject G in masterlist) 
            {
                // if object is dead, ignore it
                if (G.IsDead) 
                    continue;
                
                // otherwise perform all update logic
                else 
                {
                    AliveObjects.Add(G);
                    // if near screen add to renderlist and to any specific update lists
                    if (G.IsNearScreen()) 
                    {
                        updateAndrenderList.Add(G);

                        // differentiate between different object categories
                        if (G is Platform) 
                            terrainToUpdate.Add((Platform)G);

                        if (G is ICollisionObject)
                            interactiveObjects.Add((ICollisionObject)G);

                        if (G is Character)
                            activeCharacters.Add((Character)G);

                        if (G is MovableObject)
                            movableObjectsToUpdate.Add((MovableObject)G);
                    }
                }
            }
            // Inform the AI about terrainPieces that are relevant this frame
            Enemy.AIrelevantTerrain = terrainToUpdate;
            // Inform AI with old player position information to give player a slight edge ;)
            Enemy.playerLocation = player.BoundingBox.Center;

            foreach (GameObject O in updateAndrenderList) 
            {
                O.Update(time);
            }
            // Update player
            player.Update(time);


            // collision checks
            GroundCollisionControl.CheckGroundCollision(player, terrainToUpdate);
            // Update all other objects
            foreach (MovableObject O in movableObjectsToUpdate) 
            {
                GroundCollisionControl.CheckGroundCollision(O, terrainToUpdate);
            }


            // check for object interaction collision, such as item pick up, projectiles
            // and areaeffects
            foreach (ICollisionObject O in interactiveObjects) 
            {
                foreach (Character C in activeCharacters)
                {
                    O.CheckCollisionWithCharacter(C);
                }
            }

            // replace masterlist with list that contains only objects that are not dead
            masterlist = AliveObjects;

            // at the end of updating we want to readjust the world to that player
            // is in the middle of screen
            Camera.FocusCameraOnPlayer(masterlist ,player);
        }

        // draw all gameobjects
        public void Draw(SpriteBatch S, TextureLoader T) 
        {
            foreach (GameObject G in updateAndrenderList) 
            {
                G.Draw(S, T);
            }
            player.Draw(S,T);
        }

        // use this to add new objects during gameplay
        public void AddGameObject(GameObject G) 
        {
            // we cant add to masterlist in the middle of update
            // since masterlist will be overwritten at end of loop
            NewObjectsWaitList.Add(G);
        }
    }
}
