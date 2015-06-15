using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    interface ICharacterInteract
    {
        void CheckCollisionWithCharacter(Character C);
    }
}
