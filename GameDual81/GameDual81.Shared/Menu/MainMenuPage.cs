using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.Menu
{
    class MainMenuPage : Page
    {
        public MainMenuPage() 
        {
            MenuButton Startgame = new MenuButton("Start");
            MenuButton Character = new MenuButton("Character");
            MenuButton Options = new MenuButton("Options");
            MenuButton Exit = new MenuButton("Exit");
            

            // set the location and size for buttons
            Startgame.PositionAndSize = new Rectangle(320, 192, 640, 100);
            Character.PositionAndSize = new Rectangle(320, 322, 640, 100);
            Options.PositionAndSize = new Rectangle(320, 452, 640, 100);
            Exit.PositionAndSize = new Rectangle(440, 628, 400, 100);


            // adjust text padding
            Startgame.SetTextPadding(230,20);
            Character.SetTextPadding(100, 20);
            Options.SetTextPadding(200, 20);
            Exit.SetTextPadding(190, 20);
            

            // hook up the button events
            Startgame.onClick += containingScreen.ContainingClass.GoToGameScreen;
            Character.onClick += GoCharacterPage;
            Options.onClick += GoOptionsPage;
            Exit.onClick += containingScreen.ContainingClass.ExitApplication;
            

            // after buttons initialized, add then to our list
            buttons.Add(Startgame);
            buttons.Add(Character);
            buttons.Add(Options);
            buttons.Add(Exit);
        }


        // event handlers
        void GoOptionsPage(MenuButton B) 
        {
            containingScreen.SwitchPage(new OptionsPage());
        }
        void GoCharacterPage(MenuButton B)
        {
            containingScreen.SwitchPage(new CharacterSetupPage());
        }
    }
}
