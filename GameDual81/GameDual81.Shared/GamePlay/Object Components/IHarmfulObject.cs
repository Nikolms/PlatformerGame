using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    public interface IHarmfulObject
    {
        float Damage { get; set; }

        void HitAnObject(IDestroyableObject D);

        Rectangle GetBoundingBox();
        ObjectAlignment GetAlignment();
    }
}
