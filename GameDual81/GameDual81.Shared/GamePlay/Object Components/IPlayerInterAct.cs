using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{

    // this interface is to be used by objects that only interact with player
    // like level Exit door, pick up items and so on
    interface IInteractiveObject
    {
        void CheckPlayerCollision(Player P);
    }
}
