using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.Menu
{
    class OptionsPage : Page
    {
        public OptionsPage() 
        {
            // create buttons
            MenuButton Music = new MenuButton("Music:  ");
            MenuButton Sound = new MenuButton("Sound:  ");
            MenuButton Back = new MenuButton("Back");

            // additional information
            Music.addOnText = GameSettings.MusicSetting;
            Sound.addOnText = GameSettings.SoundSetting;

            // adjust button position
            Music.PositionAndSize = new Rectangle(320, 250, 640, 100);
            Sound.PositionAndSize = new Rectangle(320, 370, 640, 100);
            Back.PositionAndSize = new Rectangle(320, 648, 640, 100);

            // adjust text position
            Music.SetTextPadding(50,20);
            Sound.SetTextPadding(50,20);
            Back.SetTextPadding(250,20);

            // set up events
            Music.onClick += ChangeMusicSetting;
            Sound.onClick += ChangeSoundSetting;
            Back.onClick += ReturnToMainMenuPage;

            // add buttons to list
            buttons.Add(Music);
            buttons.Add(Sound);
            buttons.Add(Back);
        }

        void ReturnToMainMenuPage(MenuButton B) 
        {
            containingScreen.SwitchPage(new MainMenuPage());
        }

        // changes the bool value of the setting to the opposite
        // then adds a string to the button depending on the current setting
        void ChangeSoundSetting(MenuButton B) 
        {
            GameSettings.SoundIsOn = !GameSettings.SoundIsOn;
            B.addOnText = GameSettings.SoundSetting;
        }
        // same as above
        void ChangeMusicSetting(MenuButton B) 
        {
            GameSettings.MusicIsOn = !GameSettings.MusicIsOn;
            B.addOnText = GameSettings.MusicSetting;
        }
    }
}
