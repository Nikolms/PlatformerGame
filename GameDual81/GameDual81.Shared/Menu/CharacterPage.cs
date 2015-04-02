using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.Menu
{
    class CharacterPage : Page
    {
        public CharacterPage() 
        {
            buttons = new List<MenuButton>();

            MenuButton Back = new MenuButton("Back");


            Back.PositionAndSize = new Rectangle(320, 648, 640, 100);

            Back.SetTextPadding(250, 20);

            Back.onClick += ReturnToMainMenuPage;

            buttons.Add(Back);

        }

        void ReturnToMainMenuPage(MenuButton B) 
        {
            containingScreen.SwitchPage(new MainMenuPage());
        }
    }
}
