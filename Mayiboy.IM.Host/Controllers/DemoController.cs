using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.Host.Controllers
{
    public class DemoController : BaseApiController
    {
        private readonly IImUserInfoService _imUserInfoService;
        private readonly IChannelInfoService _channelInfoService;

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imUserInfoService"></param>
        /// <param name="channelInfoService"></param>
        public DemoController(IImUserInfoService imUserInfoService, IChannelInfoService channelInfoService)
        {
            _imUserInfoService = imUserInfoService;
            _channelInfoService = channelInfoService;
        }
        #endregion

        public object Test()
        {
            //LogManager.DefaultLogger.ErrorFormat("asdf{0}", DateTime.Now.Toyyyy_MM_dd_HH_mm_ss());

            //var res = _imUserInfoService.Find("16eafcf0-247c-4ec5-ae4f-de72a60c9cfd".ToGuid());

            //var abc= _channelInfoService.FindStorageStatus(Guid.Empty);

            CacheManager.RedisChat.HSet("234", "asdf", "asdf");
            CacheManager.RedisChat.HDel("234", "asdf");


            return "";
        }
    }
}
