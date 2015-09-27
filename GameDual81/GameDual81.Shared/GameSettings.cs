using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame
{
    abstract class GameSettings
    {
        // SoundEffects switch
        public static bool SoundIsOn { get; set; }
        // Music switch
        public static bool MusicIsOn { get; set; }
        
        // A string to display current settings on menubuttons
        public static string SoundSetting 
        { get
            { if (SoundIsOn) return "On";
            return "Off";
            }
        }

        public static string MusicSetting
        {
            get
            {
                if (MusicIsOn) return "On";
                return "Off";
            }
        }

        // accelerometer sensitivity
        // TODO add functionality to limit within certain values
        public static float Sensitivity { get; set; }


        // Player ingame stats and abilities
        public static ActionID Skill1 { get; set; }
        public static ActionID Skill2 { get; set; }
        public static ActionID Skill3 { get; set; }
        public static ActionID Skill4 { get; set; }

        public static int ArmorUpgrade { get; set; }
        public static int MeleeUpgrade { get; set; }
        public static int RangedUpgrade { get; set; }
    }
}
