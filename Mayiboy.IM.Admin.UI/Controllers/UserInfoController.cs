using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.Admin.UI.Models;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.Admin.UI.Controllers
{
    public class UserInfoController : BaseController
    {
        private readonly IImUserInfoService _imUserInfoService;
        private readonly IGroupInfoService _groupInfoService;
        private readonly IUserGroupService _userGroupService;

        #region ctor
        public UserInfoController(IImUserInfoService imUserInfoService, IGroupInfoService groupInfoService, IUserGroupService userGroupService)
        {
            _imUserInfoService = imUserInfoService;
            _groupInfoService = groupInfoService;
            _userGroupService = userGroupService;
        } 
        #endregion

        // GET: UserInfo
        public ActionResult Index()
        {
            ViewBag.GroupList = _groupInfoService.QueryAll();
            return View();
        }

        #region 查询用户信息
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="imUserName"></param>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="groupId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult Query(string imUserName, string userId, string userType, string groupId, int page = 1, int limit = 20)
        {
            var request = new QueryImUserInfoRequest();
            request.ImUserName = imUserName;
            request.UserId = userId;
            request.UserType = (userType.IsNullOrEmpty() ? null : (int?)userType.ToInt32());
            request.GroupId = (groupId.IsNullOrEmpty() ? null : (Guid?)groupId.ToGuid());
            request.PageIndex = page;
            request.PageSize = limit;

            var response = _imUserInfoService.QueryImUserInfo(request);

            #region 用户标识设置掩码
            if (response.IsSuccess && response.EntityList != null)
            {
                response.EntityList.ForEach(e =>
                {
                    e.UserHeadimg = string.IsNullOrEmpty(e.UserHeadimg) ? "../Content/Images/defaultimg.jpg" : e.UserHeadimg;
                });
            }
            #endregion

            return ToJsonResult(new { code = 0, data = response.EntityList, count = response.TotalCount });
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Save(ImUserInfoModel model)
        {
            if (model.IsNull())
            {
                return ToJsonErrorResult(1, "参数不能为空");
            }

            var dto = _imUserInfoService.Save(new ImUserInfoDto
            {
                ImUserId = model.ImUserId.ToGuid(),
                ImUserName = model.ImUserName,
                UserHeadimg = model.UserHeadimg,
                UserId = model.UserId,
                UserType = model.UserType.ToInt32(),
                Gender = (model.Gender.IsNullOrEmpty() ? null : (int?)model.Gender.ToInt32()),
                Remark = model.Remark
            });

            if (dto.ImUserId.IsNullGuid())
            {
                return ToJsonErrorResult(2, "保存用户信息出错");
            }
            else
            {
                return ToJsonResult(new { status = 0, msg = "ok" });
            }
        }

        #region 删除用户
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="imuserId">用户标识</param>
        /// <returns></returns>
        public ActionResult Del(string imuserId)
        {
            var num = _imUserInfoService.Del(imuserId.ToGuid());
            if (num > 0)
            {
                return ToJsonResult(new { status = 0, msg = "ok" });
            }
            else
            {
                return ToJsonErrorResult(2, "删除用户失败");
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imUserId"></param>
        /// <param name="groupname"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult QueryGroup(string imUserId, string groupname, int page = 1, int limit = 20)
        {
            var request = new QueryGroupRequest();
            request.ImUserId = imUserId.ToGuid();
            request.GroupName = groupname;
            request.PageIndex = page;
            request.PageSize = limit;

            var response = _imUserInfoService.QueryGroup(request);

            if (response.IsSuccess && response.EntityList != null)
            {
                response.EntityList.ForEach(e =>
                {
                    e.GroupPicture = string.IsNullOrEmpty(e.GroupPicture) ? "../Content/Images/defaultimg.jpg" : e.GroupPicture;
                });
            }

            return ToJsonResult(new { code = 0, data = response.EntityList.Select(e => e.As<GroupUserModel>()).ToList(), count = response.TotalCount });
        }

        #region 删除组用户
        /// <summary>
        /// 删除组用户
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public ActionResult DelGroup(string userGroupId)
        {
            int num = _userGroupService.Del(userGroupId.ToGuid());
            if (num > 0)
            {
                return ToJsonResult(new { status = 0, msg = "ok" });
            }
            else
            {
                return ToJsonErrorResult(2, "删除失败");
            }
        }
        #endregion

        #region 保存用户组
        /// <summary>
        /// 保存用户组
        /// </summary>
        /// <param name="imUserId">用户标识</param>
        /// <param name="groupIds">组标识集合</param>
        /// <param name="ischecked">选中状态</param>
        /// <returns></returns>
        public ActionResult SaveUserGroup(string imUserId, List<string> groupIds, string ischecked)
        {
            if (imUserId.IsNullOrEmpty() || groupIds == null || groupIds.Count == 0 || ischecked.IsNullOrEmpty())
            {
                return ToJsonErrorResult(1, "参数不能为空");
            }

            var num = _userGroupService.SaveUserGroup(imUserId.ToGuid(), groupIds.Select(Guid.Parse).ToList(), bool.Parse(ischecked));

            if (num == 0)
            {
                return ToJsonErrorResult(1, "保存失败");
            }
            else
            {
                return ToJsonResult(new { status = 0, msg = "ok" });
            }
        }
        #endregion
    }
}