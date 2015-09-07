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
using ThielynGame.Screens;
using ThielynGame.GamePlay.EnemyTypes;

namespace ThielynGame.GamePlay
{
    class LevelManager
    {
        public bool gameOver { get; private set; }

        // this flag is checked to know loading screen needs to display
        public bool IsCreatingLevel { get; private set; }


        // stores all the gameobjects currently in the game
        static List <GameObject> masterlist = null;

        // this list is needed so that we can add gameobjects mid update loop
        // and not lose them before next frame when masterlist is overwritten
        static List<GameObject> NewObjectsWaitList = new List<GameObject>();

        List<GameObject> updateAndrenderList = new List<GameObject>();

        // we need to keep track of player separately too
        Player player;

        // keep track of current level
        int levelCounter = 0;

        public void Initialize(Player player) 
        {
            levelCounter = 0;
            masterlist = new List<GameObject>();
            this.player = player;
            
            StartNewLevel();
        }

        public void StartNewLevel() 
        {
            // overwrite masterlist everytime a new level is to be created
            masterlist = new List<GameObject>();
            NewObjectsWaitList = new List<GameObject>();

            Task t = new Task(CreateNewLevel);
            t.Start();
        }

        async void CreateNewLevel() 
        {
            IsCreatingLevel = true;
            levelCounter ++;
            LevelGeneratorObject G = new LevelGeneratorObject(16);
            G.getLevelInfo(masterlist);

            // TESTING, REMOVE
            Enemy e1 = new Slime(new Vector2(1000,200));
            Enemy e2 = new Slime(new Vector2(2000, 200));
            Enemy e3 = new Slime(new Vector2(1500, 200));
            Enemy e4 = new Slime(new Vector2(3000, 200));
            Enemy e5 = new Slime(new Vector2(4000, 200));
            Enemy e6 = new Slime(new Vector2(3500, 200));

            AddGameObject(e1);
            AddGameObject(e2);
            AddGameObject(e3);
            AddGameObject(e4);
            AddGameObject(e5);
            AddGameObject(e6);

            await Task.Delay(TimeSpan.FromSeconds(2));

            IsCreatingLevel = false;
        }

        //all game objects are updated in this method
        public void Update(TimeSpan time) 
        {
            // add new objects to masterlist
            masterlist.AddRange(NewObjectsWaitList);
            // clear the new objects waitinglist
            NewObjectsWaitList.Clear();

            // a list that stores all objects that are not dead, 
            List<GameObject> AliveObjects = new List<GameObject>();
            // a list for all the terrain that is near screen this frame
            List<Platform> terrainToUpdate = new List<Platform>();
            // all movable objects that are near screen
            List<PhysicsObjects> movableObjectsToUpdate = new List<PhysicsObjects>();
            List<HarmfullObject> interactiveObjects = new List<HarmfullObject>();
            List<IInteractiveObject> playerInteractionObjects = new List<IInteractiveObject>();
            // all enemies that need to update and check collisions this frame
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

                        if (G is IInteractiveObject)
                            playerInteractionObjects.Add((IInteractiveObject)G);

                        if (G is HarmfullObject)
                            interactiveObjects.Add((HarmfullObject)G);

                        if (G is Character)
                            activeCharacters.Add((Character)G);

                        if (G is PhysicsObjects)
                            movableObjectsToUpdate.Add((PhysicsObjects)G);
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
            foreach (PhysicsObjects O in movableObjectsToUpdate) 
            {
                GroundCollisionControl.CheckGroundCollision(O, terrainToUpdate);
            }

            foreach(IInteractiveObject P in playerInteractionObjects) 
            {
                P.CheckPlayerCollision(player);
            }

            // check for object interaction collision, such as item pick up, projectiles
            // and areaeffects
            foreach (HarmfullObject O in interactiveObjects) 
            {
                O.CheckCollisionWithCharacter(player);

                foreach (Character C in activeCharacters)
                {
                    O.CheckCollisionWithCharacter(C);
                }
            }

            // replace masterlist with list that contains only objects that are not dead
            masterlist = AliveObjects;

            // at the end of updating we want to readjust the world to that player
            // is in the middle of screen
            Camera.FocusCameraOnPlayer(masterlist, player); 

            // if player is dead exit game screen
            if (player.IsDead) gameOver = true;
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
