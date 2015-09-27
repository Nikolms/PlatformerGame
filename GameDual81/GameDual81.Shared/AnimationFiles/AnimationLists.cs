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
                new FrameObject() {durationMillisec = 200, frameSource = new Rectangle(135,0,90,70)}
            };

            List<FrameObject> player_spellcast = new List<FrameObject>()
            {
                new FrameObject() { durationMillisec = 100, frameSource = new Rectangle(45,70,45,70)}
            };

            #endregion

            #region troll animation

            List<FrameObject> Troll_run = new List<FrameObject>()
            {
                new FrameObject() { durationMillisec = 120, frameSource = new Rectangle(0,0,80,120) },
                new FrameObject() { durationMillisec = 120, frameSource = new Rectangle(80,0,80,120) },
                new FrameObject() { durationMillisec = 120, frameSource = new Rectangle(160,0,80,120) },
                new FrameObject() { durationMillisec = 120, frameSource = new Rectangle(240,0,80,120) }
            };


            List<FrameObject> Troll_idle = new List<FrameObject>()
            {
                new FrameObject() {durationMillisec = 500, frameSource = new Rectangle(0,0,80,120) }
            };

            List<FrameObject> troll_melee = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 1000, frameSource = new Rectangle(0,0,80,120)},
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(0,0,80,120)}
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
            List<FrameObject> snake_run = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 250, frameSource = new Rectangle(0,90,80,90)},
                new FrameObject() {durationMillisec = 250, frameSource = new Rectangle(80,90,80,90)}
            };
            
            List<FrameObject> snake_melee = new List<FrameObject>()
            {
                new FrameObject() {durationMillisec = 500, frameSource = new Rectangle(160,90,80,90)},
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(240,90,160,90)}
            };
            

            #endregion

            #region archer animation

            List<FrameObject> archer_idle = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(0,0,45,70)} 
            };
            List<FrameObject> archer_run = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 150, frameSource = new Rectangle(0,0,45,70) },
                new FrameObject() {durationMillisec = 150, frameSource = new Rectangle(45,0,45,70) },
                new FrameObject() {durationMillisec = 150, frameSource = new Rectangle(90,0,45,70) },
                new FrameObject() {durationMillisec = 150, frameSource = new Rectangle(45,0,45,70) }
            };
            
            List<FrameObject> archer_ranged = new List<FrameObject>() 
            {
                new FrameObject() {durationMillisec = 100, frameSource = new Rectangle(0,70,45,70)}
            };

            #endregion

            #region StatusEffect animations

            List<FrameObject> poison_effect = new List<FrameObject>()
            {
                new FrameObject() {durationMillisec = 120, frameSource = new Rectangle(0,0,45,70) },
                new FrameObject() {durationMillisec = 120, frameSource = new Rectangle(45,0,45,70) },
                new FrameObject() {durationMillisec = 120, frameSource = new Rectangle(90,0,45,70) },
                new FrameObject() {durationMillisec = 120, frameSource = new Rectangle(135,0,45,70) },
                new FrameObject() {durationMillisec = 120, frameSource = new Rectangle(180,0,45,70) },
            };

            List<FrameObject> fireCloak = new List<FrameObject>()
            {
                new FrameObject() { durationMillisec = 150, frameSource = new Rectangle(0,0,90,90) },
                new FrameObject() { durationMillisec = 150, frameSource = new Rectangle(90,0,90,90) },
                new FrameObject() { durationMillisec = 150, frameSource = new Rectangle(180,0,90,90) },
                new FrameObject() { durationMillisec = 150, frameSource = new Rectangle(270,0,90,90) } 
            };

            List<FrameObject> furyEffect = new List<FrameObject>()
            {
                new FrameObject() { durationMillisec = 150, frameSource = new Rectangle(0,0,50,60) },
                new FrameObject() { durationMillisec = 150, frameSource = new Rectangle(50,0,50,60) },
                new FrameObject() { durationMillisec = 150, frameSource = new Rectangle(100,0,50,60) },
                new FrameObject() { durationMillisec = 150, frameSource = new Rectangle(150,0,50,60) }
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
            animationCollection.Add("player_melee", player_melee);
            animationCollection.Add("player_charge", player_charge);
            animationCollection.Add("player_spellcast", player_spellcast);

            animationCollection.Add("slime_idle", slime_idle);
            animationCollection.Add("slime_run", slime_run);

            animationCollection.Add("archer_idle", archer_idle);
            animationCollection.Add("archer_run", archer_run);
            animationCollection.Add("archer_ranged", archer_ranged);

            animationCollection.Add("snake_idle", snake_idle);
            animationCollection.Add("snake_run", snake_run);
            animationCollection.Add("snake_melee", snake_melee);

            animationCollection.Add("troll_idle", Troll_idle);
            animationCollection.Add("troll_run", Troll_run);
            animationCollection.Add("troll_melee", troll_melee);

            animationCollection.Add("furyEffect", furyEffect);
            animationCollection.Add("fire_cloak_effect", fireCloak);
            animationCollection.Add("poison_effect", poison_effect);

            animationCollection.Add("yellow_flash", yellow_flash);

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
