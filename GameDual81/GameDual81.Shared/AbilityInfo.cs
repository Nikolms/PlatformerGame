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
            SkillData FireCloak = new SkillData()
            {
                imageSourcePosition = new Rectangle(200, 100, 100, 100), actionID = ActionID.FireCloak,
                CoolDown = 10000, PreExecuteDuration = 1000, PostExecutionDuration = 0,
                EffectDuration = 5000, DamageModifier = 2f, EffectStrength = 0, SecondaryEffectStrenght = 0
            };
            SkillData GhostWalk = new SkillData()
            {
                imageSourcePosition = new Rectangle(100, 100, 100, 100), actionID = ActionID.GhostWalk,
                CoolDown = 15000, PreExecuteDuration = 0, PostExecutionDuration = 0,
                EffectDuration = 3000, DamageModifier = 0f, EffectStrength = 0, SecondaryEffectStrenght = 0
            };
            SkillData IceBolt = new SkillData()
            {
                imageSourcePosition = new Rectangle(300, 0, 100, 100), actionID = ActionID.IceBolt,
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
            SkillLibrary.Add(ActionID.Charge, Charge);
            SkillLibrary.Add(ActionID.FireCloak, FireCloak);
            SkillLibrary.Add(ActionID.GhostWalk, GhostWalk);
            SkillLibrary.Add(ActionID.IceBolt, IceBolt);
            SkillLibrary.Add(ActionID.Regenerate, Regenerate);
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
