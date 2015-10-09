using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.Menu
{
    public delegate void SkillSelectAction (SkillSelectButton S);

    class CharacterSetupPage : Page
    {
        Page SkillMenuPopup;
        SkillSelectButton buttonToUpdate;


        // Constructor
        public CharacterSetupPage()
        {
            MenuButton Back = new MenuButton("Back");

            SkillSelectButton skillOneSelection = new SkillSelectButton();
            SkillSelectButton skillTwoSelection = new SkillSelectButton();
            SkillSelectButton skillThreeSelection = new SkillSelectButton();

            Back.PositionAndSize = new Rectangle(440,600,400,100);
            skillOneSelection.PositionAndSize = new Rectangle(100, 100, 100, 100);
            skillTwoSelection.PositionAndSize = new Rectangle(100, 250, 100, 100);
            skillThreeSelection.PositionAndSize = new Rectangle(100, 400, 100, 100);

            Back.onClick += BackButton;
            skillOneSelection.onClickEvent += OpenPopup;
            skillTwoSelection.onClickEvent += OpenPopup;
            skillThreeSelection.onClickEvent += OpenPopup;

            skillOneSelection.Tier = 1;
            skillTwoSelection.Tier = 2;
            skillThreeSelection.Tier = 3;

            skillOneSelection.actionID = GameSettings.Skill1;
            skillTwoSelection.actionID = GameSettings.Skill2;
            skillThreeSelection.actionID = GameSettings.Skill3;

            buttons.Add(Back);
            buttons.Add(skillOneSelection);
            buttons.Add(skillTwoSelection);
            buttons.Add(skillThreeSelection);
        }


        public override void checkButtonClick(List<Vector2> inputLocations)
        {
            // if the skill selection popup is open direct all input to it
            if (SkillMenuPopup != null)
                SkillMenuPopup.checkButtonClick(inputLocations);

            else 

            base.checkButtonClick(inputLocations);
        }

        public override void Draw(SpriteBatch S)
        {
            if (SkillMenuPopup != null) SkillMenuPopup.Draw(S);
            base.Draw(S);
        }

        // PAGE BUTTON METHODS
        void BackButton(MenuButton M)
        {
            containingScreen.SwitchPage(new MainMenuPage());
        }
        void OpenPopup(SkillSelectButton M)
        {
            buttonToUpdate = M;
            SkillMenuPopup = new SkillSelectionPopup(M, ClosePopup);
        }
        public void ClosePopup(SkillSelectButton M)
        {
            buttonToUpdate.actionID = M.actionID;
            SkillMenuPopup = null;

            if (buttonToUpdate.Tier == 1) GameSettings.Skill1 = M.actionID;
            if (buttonToUpdate.Tier == 2) GameSettings.Skill2 = M.actionID;
            if (buttonToUpdate.Tier == 3) GameSettings.Skill3 = M.actionID;

            buttonToUpdate = null;
        }
        
    }



    public class SkillSelectButton : BaseButton
    {
        public int Tier;
        public SkillSelectAction onClickEvent;
        public ActionID actionID;
        

        public override bool CheckIfClicked(Vector2 inputLocation)
        {
            if (base.CheckIfClicked(inputLocation))
                onClickEvent(this);

            return true;
        }

        public override void Draw(SpriteBatch S)
        {
            imageSourceLocation = SkillData.GetSkillData(actionID).imageSourcePosition;

            S.Draw(CommonAssets.skillMenuIcons,
                clickArea, imageSourceLocation,
                Color.White);
        }
    }



    class SkillSelectionPopup : Page
    {
        Rectangle popupPosition = new Rectangle(200,20,600,700);

        // These lists are important for balance        
        List<ActionID> Tier1 = new List<ActionID>
        { ActionID.BattleRage, ActionID.ArcaneCloak,
          ActionID.ArcaneBolt, ActionID.ChainSpear,
            ActionID.Heal, ActionID.ArcaneSpray };

        List<ActionID> Tier2 = new List<ActionID>
        { ActionID.Charge, ActionID.Regenerate,
          ActionID.ArcaneLightning};

        List<ActionID> Tier3 = new List<ActionID>
        { ActionID.ShadowForm, ActionID.ArcaneStorm, ActionID.LifeSteal};

        public SkillSelectionPopup(SkillSelectButton activator, SkillSelectAction buttonAction)
        {
            List<ActionID> tempList = new List<ActionID>();

            // check the requesting buttons tier and give a list of skills available in that slot
            if (activator.Tier >= 1) tempList.AddRange(Tier1);
            if (activator.Tier >= 2) tempList.AddRange(Tier2);
            if (activator.Tier >= 3) tempList.AddRange(Tier3);

            // request all the skills available for requested Tier
            // then create as many buttons and adjust position for each per loop
            SkillData buttonInfo = new SkillData();
            int Y_adjust = 0, X_adjust = 0;

            foreach (ActionID id in tempList)
            {
                SkillSelectButton button = new SkillSelectButton();
                button.actionID = id;
                button.onClickEvent += buttonAction;
                button.PositionAndSize = new Rectangle(300 + X_adjust, 70 + Y_adjust, 100,100);

                buttons.Add(button);
                Y_adjust += 130;

                // move to next column every 4th loop
                if (Y_adjust == 520)
                {
                    X_adjust += 130;
                    Y_adjust = 0;
                } 
            }
        }


        public override void Draw(SpriteBatch S)
        {
            S.Draw(CommonAssets.PopupBackground, popupPosition, Color.White);
            base.Draw(S);
        }
    }
}
