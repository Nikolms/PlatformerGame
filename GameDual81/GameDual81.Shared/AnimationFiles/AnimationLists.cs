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
            List<FrameObject> player_Idle = new List<FrameObject>();
                FrameObject PA = new FrameObject(); PA.durationMillisec = 2000; PA.frameSource = new Rectangle(0, 180, 60, 90);
                FrameObject PB = new FrameObject(); PB.durationMillisec = 150; PB.frameSource = new Rectangle(60, 180, 60, 90);
                FrameObject PC = new FrameObject(); PC.durationMillisec = 150; PC.frameSource = new Rectangle(120, 180, 60, 90);
                FrameObject PD = new FrameObject(); PD.durationMillisec = 150; PD.frameSource = new Rectangle(60, 180, 60, 90);
            player_Idle.Add(PA); player_Idle.Add(PB); player_Idle.Add(PC); player_Idle.Add(PD);

            List<FrameObject> player_Run_Left = new List<FrameObject>();
            FrameObject PE = new FrameObject(); PE.durationMillisec = 150; PE.frameSource = new Rectangle(0, 90, 60, 90);
            FrameObject PF = new FrameObject(); PF.durationMillisec = 150; PF.frameSource = new Rectangle(60, 90, 60, 90);
            FrameObject PG = new FrameObject(); PG.durationMillisec = 150; PG.frameSource = new Rectangle(120, 90, 60, 90);
            FrameObject PH = new FrameObject(); PH.durationMillisec = 150; PH.frameSource = new Rectangle(180, 90, 60, 90);
            player_Run_Left.Add(PE); player_Run_Left.Add(PF); player_Run_Left.Add(PG); player_Run_Left.Add(PH);

            List<FrameObject> player_Run_Right = new List<FrameObject>();
            FrameObject PI = new FrameObject(); PI.durationMillisec = 150; PI.frameSource = new Rectangle(0, 0, 60, 90);
            FrameObject PJ = new FrameObject(); PJ.durationMillisec = 150; PJ.frameSource = new Rectangle(60, 0, 60, 90);
            FrameObject PK = new FrameObject(); PK.durationMillisec = 150; PK.frameSource = new Rectangle(120, 0, 60, 90);
            FrameObject PL = new FrameObject(); PL.durationMillisec = 150; PL.frameSource = new Rectangle(180, 0, 60, 90);
            player_Run_Right.Add(PI); player_Run_Right.Add(PJ); player_Run_Right.Add(PK); player_Run_Right.Add(PL);

            List<FrameObject> player_Melee_Right = new List<FrameObject>();
            FrameObject PM1 = new FrameObject(); PM1.durationMillisec = 100; PM1.frameSource = new Rectangle(0, 0, 140, 120);
            FrameObject PM2 = new FrameObject(); PM2.durationMillisec = 100; PM2.frameSource = new Rectangle(140, 0, 140, 120);
            FrameObject PM3 = new FrameObject(); PM3.durationMillisec = 100; PM3.frameSource = new Rectangle(280, 0, 140, 120);
            FrameObject PM4 = new FrameObject(); PM4.durationMillisec = 100; PM4.frameSource = new Rectangle(420, 0, 140, 120);
            player_Melee_Right.Add(PM1); player_Melee_Right.Add(PM2); player_Melee_Right.Add(PM3); player_Melee_Right.Add(PM4);

            List<FrameObject> player_Melee_Left = new List<FrameObject>();
            FrameObject PM5 = new FrameObject(); PM5.durationMillisec = 100; PM5.frameSource = new Rectangle(0, 120, 140, 120);
            FrameObject PM6 = new FrameObject(); PM6.durationMillisec = 100; PM6.frameSource = new Rectangle(140, 120, 140, 120);
            FrameObject PM7 = new FrameObject(); PM7.durationMillisec = 100; PM7.frameSource = new Rectangle(290, 120, 140, 120);
            FrameObject PM8 = new FrameObject(); PM8.durationMillisec = 100; PM8.frameSource = new Rectangle(420, 120, 140, 120);
            player_Melee_Left.Add(PM5); player_Melee_Left.Add(PM6); player_Melee_Left.Add(PM7); player_Melee_Left.Add(PM8);

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

            #endregion

            // add all animations to dictionary
            animationCollection.Add("player_idle", player_Idle);
            animationCollection.Add("player_run_left", player_Run_Left);
            animationCollection.Add("player_run_right", player_Run_Right);
            animationCollection.Add("player_melee_left", player_Melee_Left);
            animationCollection.Add("player_melee_right", player_Melee_Right);

            animationCollection.Add("troll_idle", Troll_idle);
            animationCollection.Add("troll_run_left", Troll_run_left);
            animationCollection.Add("troll_run_right", Troll_run_right);

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
