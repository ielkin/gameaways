using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameMiner.BusinessLayer
{
    public class JsonService : HttpService, IHttpService
    {
        public JsonService()
            : base(new JsonSerializer())
        {
        }
    }
}
