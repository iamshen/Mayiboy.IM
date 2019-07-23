using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mayiboy.IM.UI.Controllers
{
    public class TempGroupController : Controller
    {
        // GET: TempGroup
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 检查群组标识
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public ActionResult Login(string groupid)
        {
            return View();
        }
    }
}