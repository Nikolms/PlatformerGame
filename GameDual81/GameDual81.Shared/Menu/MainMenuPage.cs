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
            MenuButton Options = new MenuButton("Options");
            MenuButton Exit = new MenuButton("Exit");
            

            // set the location and size for buttons
            Startgame.PositionAndSize = new Rectangle(320,250,640,100);
            Exit.PositionAndSize = new Rectangle(320, 370, 640, 100);
            Options.PositionAndSize = new Rectangle(320, 490, 640, 100);
            

            // adjust text padding
            Startgame.SetTextPadding(230,20);
            Exit.SetTextPadding(190, 20);
            Options.SetTextPadding(200,20);

            // hook up the button events
            Startgame.onClick += containingScreen.ContainingClass.GoToGameScreen;
            Exit.onClick += containingScreen.ContainingClass.ExitApplication;
            Options.onClick += GoOptionsPage;

            // after buttons initialized, add then to our list
            buttons.Add(Startgame);
            buttons.Add(Exit);
            buttons.Add(Options);
        }


        // event handlers
        void GoOptionsPage(MenuButton B) 
        {
            containingScreen.SwitchPage(new OptionsPage());
        }
    }
}
