using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.AnimationFiles
{
    class AnimationLists
    {
        // character animations must be named 
        // character type, animation state, direction : player_run_left

        // a dictionary to store all the different animation sequences
        static Dictionary<string, List<FrameObject>> animationCollection;

        public static void Initialize() 
        {
            animationCollection = new Dictionary<string, List<FrameObject>>();

            #region player animations
            List<FrameObject> player_idle = new List<FrameObject>() 
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,1,98,98)}
                };

            List<FrameObject> player_run = new List<FrameObject>()
                {
                    new FrameObject () {durationMillisec = 250, frameSource = new Rectangle(103,1,98,98)},
                    new FrameObject () {durationMillisec = 250, frameSource = new Rectangle(205,1,98,98)},
                    new FrameObject () {durationMillisec = 250, frameSource = new Rectangle(103,1,98,98)},
                    new FrameObject () {durationMillisec = 250, frameSource = new Rectangle(205,1,98,98)}
                }; 

            List<FrameObject> player_jump = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(115,133,100,100)}
                };

            List<FrameObject> player_fall = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(6,132,100,100)}
                };

            List<FrameObject> player_spell = new List<FrameObject>()
                {
                    new FrameObject () {durationMillisec = 250, frameSource = new Rectangle(1,103,98,98)},
                    new FrameObject () {durationMillisec = 250, frameSource = new Rectangle(103,103,98,98)},
                    new FrameObject () {durationMillisec = 250, frameSource = new Rectangle(205,103,98,98)},
                    new FrameObject () {durationMillisec = 250, frameSource = new Rectangle(307,103,98,98)}
                };

            List<FrameObject> player_melee = new List<FrameObject>()
                {
                    new FrameObject () {durationMillisec = 100, frameSource = new Rectangle(6,132,100,100)},
                };
           
            #endregion

            #region cannonMonster

            List<FrameObject> cannonmonster_idle = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,1,123,78)}
                };

            List<FrameObject> cannonmonster_run = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,1,123,78)},
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,83,123,78)},
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(128,83,123,78)},
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,83,123,78)}
                };

            List<FrameObject> cannonmonster_fall = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,1,123,78)}
                };

            List<FrameObject> cannonmonster_jump = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,1,123,78)}
                };

            List<FrameObject> cannonmonster_shoot = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,165,123,78)}
                };

            #endregion

            #region swarm
            List<FrameObject> swarm_idle = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,1,13,13)}
                };

            List<FrameObject> swarm_run = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,1,11,11)}
                };

            List<FrameObject> swarm_jump = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,1,11,11)}
                };

            List<FrameObject> swarm_fall = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,1,11,11)}
                };
            #endregion

            #region FloatingMine

            List<FrameObject> floatingmine_explode = new List<FrameObject>()
            {
                new FrameObject() { durationMillisec = 100, frameSource = new Rectangle(0,0,60,60) },
                new FrameObject() { durationMillisec = 100, frameSource = new Rectangle(0,0,55,55) },
                new FrameObject() { durationMillisec = 100, frameSource = new Rectangle(0,0,50,50) },
                new FrameObject() { durationMillisec = 100, frameSource = new Rectangle(0,0,45,45) },
                new FrameObject() { durationMillisec = 100, frameSource = new Rectangle(0,0,40,40) }
            };

            #endregion

            #region dummy small

            List<FrameObject> dummy_small_run = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,15,15)}
                };

            List<FrameObject> dummy_small_idle = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,15 ,15)}
                };

            List<FrameObject> dummy_small_fall = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,15,15)}
                };

            List<FrameObject> dummy_small_jump = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,15,15)}
                };


            #endregion

            #region dummy_medium

            List<FrameObject> dummy_medium_run = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,60,60)}
                };

            List<FrameObject> dummy_medium_idle = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,60 ,60)}
                };

            List<FrameObject> dummy_medium_fall = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,60,60)}
                };

            List<FrameObject> dummy_medium_jump = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,60,60)}
                };

            #endregion

            #region dummy large

            List<FrameObject> dummy_large_run = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,100,100)}
                };

            List<FrameObject> dummy_large_idle = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,100 ,100)}
                };

            List<FrameObject> dummy_large_fall = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,100,100)}
                };

            List<FrameObject> dummy_large_jump = new List<FrameObject>()
                {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,100,100)}
                };

            #endregion

            #region UI effects

            List<FrameObject> yellow_flash = new List<FrameObject>()
            {
                new FrameObject () {durationMillisec = 500, frameSource = new Rectangle(0,0,1,1) },
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(1,0,1,1) },
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(2,0,1,1) },
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(3,0,1,1) }
            };

            #endregion

            // add all animations to dictionary
            animationCollection.Add("player_idle", player_idle);
            animationCollection.Add("player_run", player_run);
            animationCollection.Add("player_jump", player_jump);
            animationCollection.Add("player_fall", player_fall);
            animationCollection.Add("player_spell", player_spell);
            animationCollection.Add("player_melee", player_melee);

            animationCollection.Add("cannonmonster_idle", cannonmonster_idle);
            animationCollection.Add("cannonmonster_run", cannonmonster_run);
            animationCollection.Add("cannonmonster_jump", cannonmonster_jump);
            animationCollection.Add("cannonmonster_fall", cannonmonster_fall);
            animationCollection.Add("cannonmonster_shoot", cannonmonster_shoot);

            animationCollection.Add("swarm_fall", swarm_fall);
            animationCollection.Add("swarm_jump", swarm_jump);
            animationCollection.Add("swarm_idle", swarm_idle);
            animationCollection.Add("swarm_run", swarm_run);

            animationCollection.Add("floatingmine_explode", floatingmine_explode);

            animationCollection.Add("dummy_small_idle", dummy_small_idle);
            animationCollection.Add("dummy_small_run", dummy_small_run);
            animationCollection.Add("dummy_small_jump", dummy_small_jump);
            animationCollection.Add("dummy_small_fall", dummy_small_fall);

            animationCollection.Add("dummy_medium_idle", dummy_medium_idle);
            animationCollection.Add("dummy_medium_run", dummy_medium_run);
            animationCollection.Add("dummy_medium_jump", dummy_medium_jump);
            animationCollection.Add("dummy_medium_fall", dummy_medium_fall);

            animationCollection.Add("dummy_large_idle", dummy_large_idle);
            animationCollection.Add("dummy_large_run", dummy_large_run);
            animationCollection.Add("dummy_large_jump", dummy_large_jump);
            animationCollection.Add("dummy_large_fall", dummy_large_fall);


            animationCollection.Add("yellow_flash", yellow_flash);

        }

        
        public List<FrameObject> getAnimation(string animationName) 
        {
            return animationCollection[animationName];
        }
        public static List<FrameObject> GetAnimationFrames(string animationName) 
        {
            return animationCollection[animationName];
        }

    }
}
