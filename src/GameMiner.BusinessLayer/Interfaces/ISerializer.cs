using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMiner.BusinessLayer
{
    public interface ISerializer
    {
        T Deserialize<T>(string value);
    }
}
