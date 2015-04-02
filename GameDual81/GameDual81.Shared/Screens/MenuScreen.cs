using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.Menu;

namespace ThielynGame.Screens
{
    public enum MenuState { MainMenu, Options }

    class MenuScreen : Screen
    {
        public bool hasLoaded { get; set; }
        public Game1 parent { get;  set; }

        MenuState pageState;

        public static ContentManager content { get; private set; }

        Texture2D backGroundImage;
        Page currentPage;

        public MenuScreen() 
        {
            // register this instance as container for all menupages
            Page.containingScreen = this;
        }

        public static void Initialize(ContentManager CM)
        {
            content = CM;
        }

        public override void Load()
        {
            backGroundImage = content.Load<Texture2D>("menubackground");
            // set the start page as mainmenu
            currentPage = new MainMenuPage();

            hasLoaded = true;
        }

        public override void HandleInput(InputHandler input)
        {
            //this screen sends the input to currentpage in order to iterate all
            //available buttons
            currentPage.checkButtonClick(input.InputLocations);
        }

        public override void Update(TimeSpan time)
        {

        }

        public override void Draw(SpriteBatch s)
        {
            // draw the background 
            s.Draw(backGroundImage, MyRectangle.AdjustSizeCustomRectangle(0,0,1280,768), Color.White);

            //draw the currentpage on top of the background
            currentPage.Draw(s);
        }

        public override void ExitScreen()
        {
            content.Unload();
        }

        #region Page methods

        public void SwitchPage(Page nextPage) 
        {
            currentPage = nextPage;
        }

        void StartGame() 
        {

        }

        #endregion

    }
}
