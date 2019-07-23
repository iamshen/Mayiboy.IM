using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mayiboy.IM.Host.Controllers
{
    public class WsController : ApiController
    {
        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Connect")]
        public string Post()
        {
            return "1";
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        [HttpGet, Jsonp]
        [ActionName("Connect")]
        public string Get()
        {
            return "1";
        }
    }
}
