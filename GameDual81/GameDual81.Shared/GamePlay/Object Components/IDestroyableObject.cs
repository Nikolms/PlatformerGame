using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    public interface IDestroyableObject
    {
        void HitByHarmfulObject(IHarmfulObject O);

        Rectangle GetBoundingBox();
        TeamID GetTeamID();
    }
}
