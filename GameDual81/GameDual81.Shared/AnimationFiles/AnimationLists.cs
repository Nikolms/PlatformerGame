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
            new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(0,0,45,70)}
            };

            List<FrameObject> player_run = new List<FrameObject>()
            {
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(45,0,45,70)},
                new FrameObject () {durationMillisec = 200, frameSource = new Rectangle(90,0,45,70)}
            };

            List<FrameObject> player_melee = new List<FrameObject>()
            {
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(0,70,65,90)},
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(64,70,80,70)},
            };

            List<FrameObject> player_charge = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(64,139,55,80)}
            };

            #endregion

            #region troll animation

            List<FrameObject> Troll_run_left = new List<FrameObject>();
            FrameObject T1 = new FrameObject(); T1.durationMillisec = 200; T1.frameSource = new Rectangle(0, 0, 80, 120);
            FrameObject T2 = new FrameObject(); T2.durationMillisec = 200; T2.frameSource = new Rectangle(80, 0, 80, 120);
            FrameObject T3 = new FrameObject(); T3.durationMillisec = 200; T3.frameSource = new Rectangle(160, 0, 80, 120);
            FrameObject T4 = new FrameObject(); T4.durationMillisec = 200; T4.frameSource = new Rectangle(240, 0, 80, 120);
            FrameObject T5 = T3; 
            FrameObject T6 = T2;
            Troll_run_left.Add(T1); Troll_run_left.Add(T2); Troll_run_left.Add(T3); Troll_run_left.Add(T4);
            Troll_run_left.Add(T5); Troll_run_left.Add(T6);

            List<FrameObject> Troll_run_right = new List<FrameObject>();
            FrameObject T7 = new FrameObject(); T7.durationMillisec = 200; T7.frameSource = new Rectangle(0, 120, 80, 120);
            FrameObject T8 = new FrameObject(); T8.durationMillisec = 200; T8.frameSource = new Rectangle(80, 120, 80, 120);
            FrameObject T9 = new FrameObject(); T9.durationMillisec = 200; T9.frameSource = new Rectangle(160, 120, 80, 120);
            FrameObject T10 = new FrameObject(); T10.durationMillisec = 200; T10.frameSource = new Rectangle(240, 120, 80, 120);
            FrameObject T11 = T9; FrameObject T12 = T8;
            Troll_run_right.Add(T7); Troll_run_right.Add(T8); Troll_run_right.Add(T9); Troll_run_right.Add(T10);
            Troll_run_right.Add(T11); Troll_run_right.Add(T12);

            List<FrameObject> Troll_idle = new List<FrameObject>();
            FrameObject T13 = new FrameObject(); T13.durationMillisec = 500; T13.frameSource = new Rectangle(0,0,80,120);
            Troll_idle.Add(T13);

            List<FrameObject> troll_melee_right = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 1000, frameSource = new Rectangle(0,0,80,120)},
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(0,0,80,120)}
            };
            List<FrameObject> troll_melee_left = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 1000, frameSource = new Rectangle(0,0,80,120)},
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(0,0,80,120)}
            };
            #endregion

            #region slime animation

            List<FrameObject> slime_idle = new List<FrameObject> 
            {
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(0,0,70,50)}
            };

            List<FrameObject> slime_run = new List<FrameObject> 
            {
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(0,0,70,50)},
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(70,0,70,50)}
            };

            #endregion

            #region snake animation

            List<FrameObject> snake_idle = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(0,0,80,90)} 
            };
            List<FrameObject> snake_run_left = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 250, frameSource = new Rectangle(0,90,80,90)},
                new FrameObject() {durationMillisec = 250, frameSource = new Rectangle(80,90,80,90)}
            };
            List<FrameObject> snake_run_right = new List<FrameObject>()
            {
                new FrameObject() {durationMillisec = 250, frameSource = new Rectangle(0,0,80,90)},
                new FrameObject() {durationMillisec = 250, frameSource = new Rectangle(80,0,80,90)}
            };
            List<FrameObject> snake_melee_left = new List<FrameObject>()
            {
                new FrameObject() {durationMillisec = 500, frameSource = new Rectangle(160,90,80,90)},
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(240,90,160,90)}
            };
            List<FrameObject> snake_melee_right = new List<FrameObject>()
            {
                new FrameObject() {durationMillisec = 500, frameSource = new Rectangle(160,0,80,90)},
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(240,0,160,90)}
            };

            #endregion

            #region archer animation

            List<FrameObject> archer_idle = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(0,0,80,90)} 
            };
            List<FrameObject> archer_run_left = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 150, frameSource = new Rectangle(0,90,80,90)},
                new FrameObject() {durationMillisec = 150, frameSource = new Rectangle(80,90,80,90)}
            };
            List<FrameObject> archer_run_right = new List<FrameObject>()
            {
                new FrameObject() {durationMillisec = 180, frameSource = new Rectangle(0,0,80,90)},
                new FrameObject() {durationMillisec = 180, frameSource = new Rectangle(80,0,80,90)}
            };
            List<FrameObject> archer_ranged_left = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(160,90,80,90)},
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(240,90,160,90)}
            };
            List<FrameObject> archer_ranged_right = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(160,0,80,90)},
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(240,0,160,90)}
            };
#endregion

            // add all animations to dictionary
            animationCollection.Add("player_idle", player_idle);
            animationCollection.Add("player_run", player_run);
            animationCollection.Add("player_melee", player_melee);
            animationCollection.Add("player_charge", player_charge);

            animationCollection.Add("slime_idle", slime_idle);
            animationCollection.Add("slime_run", slime_run);

        }

        public List<FrameObject> getAnimation(string animationName) 
        {
            return animationCollection[animationName];
        }
        public static List<FrameObject> GetAnimation(string animationName) 
        {
            return animationCollection[animationName];
        }

    }
}
