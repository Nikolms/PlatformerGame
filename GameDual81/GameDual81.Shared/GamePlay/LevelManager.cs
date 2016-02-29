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
using ThielynGame.GamePlay.Object_Components;
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

        // we need to keep separate track of certain objects
        Player player;

        // keep track of current level
        int levelCounter = 0;

        public Rectangle renderBox_BGdeep = new Rectangle(0,0,1920,1080),
            renderBox_BGmid = new Rectangle(0,0,2560, 1080),
            renderBox_BGclose = new Rectangle(0,0,2560, 1080);


        public void Initialize(Player player) 
        {
            levelCounter = 0;
            masterlist = new List<GameObject>();
            this.player = player;
            
            StartNewLevel();
        }

        // method that initializes a new level creation, and resets certain settings
        public void StartNewLevel() 
        {
            // overwrite masterlist everytime a new level is to be created
            masterlist = new List<GameObject>();
            NewObjectsWaitList = new List<GameObject>();
            player.ResetPlayerStatus();
            Camera.Reset();

            Task t = new Task(BuildNewLevel);
            t.Start();
        }

        // method that builds the new level and its object on a separate thread
        async void BuildNewLevel() 
        {
            IsCreatingLevel = true;
            levelCounter ++;
            player.LevelUP();
            LevelGeneratorObject G = new LevelGeneratorObject(20);
            G.getLevelInfo(masterlist, levelCounter);
            

            await Task.Delay(TimeSpan.FromSeconds(1));

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
            List<IHarmfulObject> interactiveObjects = new List<IHarmfulObject>();
            List<IInteractiveObject> playerInteractionObjects = new List<IInteractiveObject>();
            // all enemies that need to update and check collisions this frame
            List<Character> activeCharacters = new List<Character>();
            List<IDestroyableObject> destroyableObjects = new List<IDestroyableObject>();

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
                            terrainToUpdate.Add( (Platform)G );

                        if (G is IInteractiveObject)
                            playerInteractionObjects.Add( (IInteractiveObject)G );

                        if (G is IHarmfulObject)
                            interactiveObjects.Add( (IHarmfulObject)G );

                        if (G is Character)
                            activeCharacters.Add( (Character)G );

                        if (G is PhysicsObjects)
                            movableObjectsToUpdate.Add( (PhysicsObjects)G );

                        if (G is IDestroyableObject)
                            destroyableObjects.Add( (IDestroyableObject)G );

                    }
                }
            }
            // Inform AI what terrain needs to be checked this frame
            Enemy.AIrelevantTerrain = terrainToUpdate;
            // Inform AI about players current position
            Enemy.playerPosition = player.Position;

            foreach (GameObject O in updateAndrenderList) 
            {
                O.Update(time);
            }
            // Update player
            player.Update(time);


            // collision checks
            foreach (IObsticle O in terrainToUpdate)
            {
                O.CheckObsticleCollision(player);

                foreach (PhysicsObjects P in movableObjectsToUpdate)
                {
                    O.CheckObsticleCollision(P);
                }
            }
            
            // Update all other objects
            foreach (PhysicsObjects O in movableObjectsToUpdate) 
            {
                //CollisionControl.CheckGroundCollision(O, terrainToUpdate);
            }

            foreach(IInteractiveObject P in playerInteractionObjects) 
            {
                P.CheckPlayerCollision(player);
            }

            // check for object interaction collision, such as item pick up, projectiles
            // and areaeffects
            foreach (IHarmfulObject HO in interactiveObjects) 
            {
                CollisionControl.CheckObjectCollision(HO, player);
                foreach (IDestroyableObject DO in destroyableObjects)
                    CollisionControl.CheckObjectCollision(HO, DO);
            }

            // replace masterlist with list that contains only objects that are not dead
            masterlist = AliveObjects;

            // at the end of updating we want to readjust the world to that player
            // is in the middle of screen
            Camera.FocusCameraOnPlayer(masterlist, player);

            // adjust background position
            renderBox_BGclose.X += Camera.GetOffsetClose(5);
            renderBox_BGmid.X += Camera.GetOffsetMid(10);

            // check if player has completed level end trigger
            if (player.ReachedEndOfLevel)
                this.StartNewLevel();

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
