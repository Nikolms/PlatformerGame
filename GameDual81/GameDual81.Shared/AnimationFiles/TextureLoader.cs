using ThielynGame.GamePlay;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.AnimationFiles
{
    public class TextureLoader
    {
        ContentManager content;

        public TextureLoader(ContentManager Content) 
        {
            content = Content;
        }

        /*
        public void preGameLoad(List<GameObject> listOfObjects) 
        {
            foreach (GameObject G in listOfObjects) 
            {
                content.Load<Texture2D>(G.TextureFileName);
            }
        } */

        public Texture2D GetTexture(string filename) 
        {
            try
            {
                return content.Load<Texture2D>(filename);
            }
            catch
            {
                return content.Load<Texture2D>("TODO");
            }
        }
    }
}
