using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay.Object_Components
{
    class DamageOnTouch
    {
        public void resolveDamage(Character C, int damage)
        {
            C.HitByAttack(damage);
        }
    }
}
