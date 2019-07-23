using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.UI.Controllers
{
    public class TempGroupController : Controller
    {
        private readonly IGroupInfoService _groupInfoService;
        private readonly IImUserInfoService _imUserInfoService;
        private readonly IUserGroupService _userGroupService;

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupInfoService"></param>
        /// <param name="imUserInfoService"></param>
        /// <param name="userGroupService"></param>
        public TempGroupController(IGroupInfoService groupInfoService, IImUserInfoService imUserInfoService,
            IUserGroupService userGroupService)
        {
            _groupInfoService = groupInfoService;
            _imUserInfoService = imUserInfoService;
            _userGroupService = userGroupService;
        }
        #endregion

        // GET: TempGroup
        public ActionResult Index(string groupid)
        {
            if (string.IsNullOrEmpty(groupid))
            {
                return Redirect("~/Page/Index?msg=群组标识不能为空");
            }

            ViewBag.GroupId = groupid;
            var entity = _groupInfoService.Find(groupid.ToGuid());
            ViewBag.GroupName = (entity == null ? "" : entity.GroupName);

            return View();
        }


        /// <summary>
        /// 检查群组标识
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public ActionResult Login(string groupid)
        {
            #region 检查群组标识有效性
            if (string.IsNullOrEmpty(groupid))
            {
                return Redirect("~/Page/Index?msg=群组标识不能为空");
            }

            //检查groupid属性
            var entity = _groupInfoService.Find(groupid.ToGuid());
            if (entity == null)
            {
                return Redirect("~/Page/Index?msg=无效群组");
            }

            if (!entity.GroupUserTypes.Contains("4"))
            {
                return Redirect("~/Page/Index?msg=非临时群组");
            }
            #endregion

            ViewBag.GroupInfo = entity;
            return View();
        }
    }
}