using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    class PlayerEmpoweredAction : CharacterAction
    {
        public PlayerEmpoweredAction(Character Actor) : base(Actor)
        {
        }

        protected override void OnExecute()
        {
            throw new NotImplementedException();
        }
    }
}
