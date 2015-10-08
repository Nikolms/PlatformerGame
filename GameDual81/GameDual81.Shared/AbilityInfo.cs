using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame
{
    class SkillData
    {
        static Dictionary<ActionID, SkillData> SkillLibrary;
        public bool SkillSelected { get; set; }

        public Rectangle imageSourcePosition { get; protected set; }
        public ActionID actionID { get; protected set; }


        // game mechanics data
        public int CoolDown { get; protected set; }
        public int PreExecuteDuration { get; protected set; }
        public int PostExecutionDuration { get; protected set; }
        public int EffectDuration { get; protected set; }
        public float DamageModifier { get; protected set; }
        public float EffectStrength { get; protected set; }
        public float SecondaryEffectStrenght { get; protected set; }
        
        

        // fills the skill info for different abilities data
        // this contains game balance related data values
        public static void Init()
        {
            SkillLibrary = new Dictionary<ActionID, SkillData>();

            SkillData ArcaneLightning = new SkillData()
            {
                imageSourcePosition = new Rectangle(100,200,100,100), actionID = ActionID.ArcaneLightning
            };

            SkillData ArcaneStorm = new SkillData()
            {
                imageSourcePosition = new Rectangle(200,200,100,100), actionID = ActionID.ArcaneStorm,
                CoolDown = 0, PreExecuteDuration = 0, PostExecutionDuration = 0,
                EffectDuration = 0, DamageModifier = 0, EffectStrength = 0, SecondaryEffectStrenght = 0 
            };

            SkillData Heal = new SkillData()
            {
                imageSourcePosition = new Rectangle(400,100,100,100), actionID = ActionID.Heal,
                CoolDown = 0, PreExecuteDuration = 0, PostExecutionDuration = 0,
                EffectDuration = 0, DamageModifier = 0, EffectStrength = 0, SecondaryEffectStrenght = 0
            };

            SkillData ChainSpear = new SkillData()
            {
                imageSourcePosition = new Rectangle(0,200,100,100), actionID = ActionID.ChainSpear,
                CoolDown = 10000, PreExecuteDuration = 0, PostExecutionDuration = 3000,
                EffectDuration = 0, DamageModifier = 1.5f, EffectStrength = 0, SecondaryEffectStrenght = 0
            };

            SkillData BattleRage = new SkillData()
            {
                imageSourcePosition = new Rectangle(300,100,100,100), actionID = ActionID.BattleRage,
                CoolDown = 15000, PreExecuteDuration = 0, PostExecutionDuration = 0,
                EffectDuration = 7000, DamageModifier = 1.2f, EffectStrength = 0, SecondaryEffectStrenght = 0 
            };
            SkillData Charge = new SkillData()
            {
                imageSourcePosition = new Rectangle(400, 0, 100, 100), actionID = ActionID.Charge,
                CoolDown = 7000, PreExecuteDuration = 0, PostExecutionDuration = 400,
                EffectDuration = 0, DamageModifier = 2f, EffectStrength = 0, SecondaryEffectStrenght = 0
            };
            SkillData ArcaneCloak = new SkillData()
            {
                imageSourcePosition = new Rectangle(200, 100, 100, 100), actionID = ActionID.ArcaneCloak,
                CoolDown = 10000, PreExecuteDuration = 1000, PostExecutionDuration = 0,
                EffectDuration = 5000, DamageModifier = 2f, EffectStrength = 0, SecondaryEffectStrenght = 0
            };
            SkillData ShadowForm = new SkillData()
            {
                imageSourcePosition = new Rectangle(100, 100, 100, 100), actionID = ActionID.ShadowForm,
                CoolDown = 15000, PreExecuteDuration = 0, PostExecutionDuration = 0,
                EffectDuration = 3000, DamageModifier = 0f, EffectStrength = 0, SecondaryEffectStrenght = 0
            };
            SkillData ArcaneBolt = new SkillData()
            {
                imageSourcePosition = new Rectangle(300, 0, 100, 100), actionID = ActionID.ArcaneBolt,
                CoolDown = 3000, PreExecuteDuration = 500, PostExecutionDuration = 0,
                EffectDuration = 0, DamageModifier = 1.4f, EffectStrength = 0, SecondaryEffectStrenght = 0
            };
            SkillData Regenerate = new SkillData()
            {
                imageSourcePosition = new Rectangle(0, 100, 100, 100), actionID = ActionID.Regenerate,
                CoolDown = 3000, PreExecuteDuration = 500, PostExecutionDuration = 0,
                EffectDuration = 0, DamageModifier = 1.4f, EffectStrength = 0, SecondaryEffectStrenght = 0
            };

            SkillLibrary.Add(ActionID.BattleRage, BattleRage);
            SkillLibrary.Add(ActionID.ChainSpear, ChainSpear);
            SkillLibrary.Add(ActionID.Charge, Charge);
            SkillLibrary.Add(ActionID.ArcaneCloak, ArcaneCloak);
            SkillLibrary.Add(ActionID.ShadowForm, ShadowForm);
            SkillLibrary.Add(ActionID.ArcaneBolt, ArcaneBolt);
            SkillLibrary.Add(ActionID.Regenerate, Regenerate);
            SkillLibrary.Add(ActionID.Heal, Heal);
            SkillLibrary.Add(ActionID.ArcaneStorm, ArcaneStorm);
            SkillLibrary.Add(ActionID.ArcaneLightning, ArcaneLightning);
        }
        
        public static SkillData GetSkillData(ActionID id)
        {
            try
            {
                return SkillLibrary[id];
            }
            catch
            {
                return SkillLibrary[ActionID.BattleRage];
            }
        }
    }
}
