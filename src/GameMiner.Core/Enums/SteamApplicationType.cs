using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMiner.Core
{
    public enum StoreApplicationType : short
    {
        Game = 0,
        Dlc = 1,
        Demo = 2,
        Movie = 3,
        Advertising = 4,
        Mod = 5,
        Hardware = 6,
        Video = 7,
        Series = 8,
        Episode = 9,
        Unknown = 255
    }
}
