using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mayiboy.IM.Admin.UI.Controllers
{
    public class BaseController : Controller
    {
        #region 错误返回
        /// <summary>
        /// 错误返回
        /// </summary>
        /// <param name="status">错误状态码</param>
        /// <param name="msg">提示消息</param>
        /// <param name="buginfo">错误提示(用于开发错误提示)</param>
        /// <returns></returns>
        public JsonResult ToJsonErrorResult(long status = 1, string msg = "有错误", string buginfo = "")
        {
            return Json(new { status = status, msg = msg, buginfo = buginfo }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 成功返回
        /// <summary>
        ///成功返回 
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <returns></returns>
        public JsonResult ToJsonResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 异常返回
        /// <summary>
        /// 异常返回
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="buginfo">错误提示(用于开发错误提示)</param>
        /// <returns></returns>
        public JsonResult ToJsonFatalResult(string msg, string buginfo = "")
        {
            return Json(new { status = -1, msg = msg, buginfo = buginfo }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}