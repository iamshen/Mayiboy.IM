using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mayiboy.IM.UI.Controllers
{
    public class PageController : Controller
    {
        /// <summary>
        /// 页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string msg = "参数无效")
        {
            ViewBag.Msg = msg;
            return View();
        }

        // GET: Page
        public ActionResult Page404()
        {
            return View();
        }
    }
}