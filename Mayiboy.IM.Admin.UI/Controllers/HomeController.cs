using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mayiboy.IM.Admin.UI.Models;

namespace Mayiboy.IM.Admin.UI.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.MenuList = GetMenuList();
            return View();
        }

        public ActionResult Page()
        {
            return View();
        }


        #region 获取菜单列表
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuInfo> GetMenuList()
        {
            List<MenuInfo> list = new List<MenuInfo>()
            {
                new MenuInfo
                {
                    Name = "群组列表",
                    Url = Url.Action("Index", "GroupInfo")
                },
                //new MenuInfo
                //{
                //    Name = "用户组列表",
                //    Url = Url.Action("Index","UserGroup")
                //},
                new MenuInfo
                {
                    Name = "用户列表",
                    Url = Url.Action("Index", "UserInfo")
                }
            };

            return list;
        }
        #endregion
    }
}