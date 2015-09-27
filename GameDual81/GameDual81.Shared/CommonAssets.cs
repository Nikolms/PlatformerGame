using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame
{
    // this class provides a global access to common textures and Fonts that are needed 
    // in a wide variety of the application
    class CommonAssets
    {
        public static Texture2D LoadingBackGround { get; set; }

        public static SpriteFont menuFont { get; set; }
        public static Texture2D menuButtonBackground { get; set; }
        public static Texture2D skillMenuIcons { get; set; }
        public static Texture2D PopupBackground { get; set; }

        // TODO for dummy uses only
        public static Texture2D TODO { get; set; }
    }
}
