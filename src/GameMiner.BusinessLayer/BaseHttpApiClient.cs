using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMiner.BusinessLayer.Steam
{
    public abstract class BaseHttpApiClient
    {
        protected IHttpService _jsonService;

        public BaseHttpApiClient(IHttpService jsonService)
        {
            _jsonService = jsonService;
        }
    }
}
