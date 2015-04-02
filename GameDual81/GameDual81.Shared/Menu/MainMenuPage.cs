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
            buttons = new List<MenuButton>();

            MenuButton Startgame = new MenuButton("Start");
            MenuButton Character = new MenuButton("Character");
            MenuButton Options = new MenuButton("Options");
            

            // set the location and size for buttons
            Startgame.PositionAndSize = new Rectangle(320,250,640,100);
            Character.PositionAndSize = new Rectangle(320, 370, 640, 100);
            Options.PositionAndSize = new Rectangle(320, 490, 640, 100);
            

            // adjust text padding
            Startgame.SetTextPadding(230,20);
            Character.SetTextPadding(190,20);
            Options.SetTextPadding(200,20);

            // hook up the button events
            Startgame.onClick += containingScreen.host.StartGameScreen;
            Character.onClick += GoCharacterPage;
            Options.onClick += GoOptionsPage;

            // after buttons initialized, add then to our list
            buttons.Add(Startgame);
            buttons.Add(Character);
            buttons.Add(Options);
        }


        // event handlers
        void GoOptionsPage(MenuButton B) 
        {
            containingScreen.SwitchPage(new OptionsPage());
        }
        void GoCharacterPage(MenuButton B) 
        {
            containingScreen.SwitchPage(new CharacterPage());
        }
    }
}
