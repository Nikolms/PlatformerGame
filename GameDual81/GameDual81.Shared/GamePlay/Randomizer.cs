using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    class Randomizer
    {
        static Random _random;

        public static void ReSeed()
        {
            _random = new Random();
        }

        public static Random Random
        { get {
                if (_random == null)
                    _random = new Random();

                return _random; }
        }
    }
}
