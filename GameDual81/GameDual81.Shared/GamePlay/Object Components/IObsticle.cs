using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay.Object_Components
{
    public struct CollisionDetailObject 
    {
        public int correctionValueX;
        public int correctionValueY;
    }
    interface IObsticle
    {
        void CheckObsticleCollision(MovableObject P);
    }
}
