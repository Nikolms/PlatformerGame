using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    interface IHarmfulObject
    {
        void CheckCollisionWithCharacter(Character C);
    }
}
