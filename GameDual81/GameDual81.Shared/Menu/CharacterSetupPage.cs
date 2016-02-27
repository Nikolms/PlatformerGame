using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;


namespace ThielynGame.Menu
{
    class CharacterSetupPage : Page
    {
        // Constructor
        public CharacterSetupPage()
        {
            MenuButton Back = new MenuButton("Back");

            buttons.Add(Back);
        }

        // PAGE BUTTON METHODS
        void BackButton(MenuButton M)
        {
            containingScreen.SwitchPage(new MainMenuPage());
        }
    }
}
