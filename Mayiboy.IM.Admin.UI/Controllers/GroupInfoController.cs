using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.Admin.UI.Controllers
{
    public class GroupInfoController : BaseController
    {
        private readonly IGroupInfoService _groupInfoService;
        private readonly IImUserInfoService _imUserInfoService;
        private readonly IUserGroupService _userGroupService;
        private readonly IChannelInfoService _channelInfoService;

        #region ctor
        public GroupInfoController(IGroupInfoService groupInfoService, IImUserInfoService imUserInfoService,
            IUserGroupService userGroupService, IChannelInfoService channelInfoService)
        {
            _groupInfoService = groupInfoService;
            _imUserInfoService = imUserInfoService;
            _userGroupService = userGroupService;
            _channelInfoService = channelInfoService;
        } 
        #endregion

        // GET: GroupInfo
        public ActionResult Index()
        {
            return View();
        }

        #region 保存组信息

        /// <summary>
        /// 保存组信息
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <param name="groupName">组名</param>
        /// <param name="groupMaxCapacity">组内用户上限</param>
        /// <param name="groupPicture">组图片</param>
        /// <param name="groupUserTypes">组内用户类型</param>
        /// <param name="storageStatus">持久化状态</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public ActionResult Save(string groupId, string groupName, string groupMaxCapacity, string groupPicture, string groupUserTypes, string storageStatus, string remark)
        {
            var dto = new GroupInfoDto();
            dto.GroupId = groupId.ToGuid();
            dto.GroupName = groupName;
            dto.GroupMaxCapacity = groupMaxCapacity.ToInt32();
            dto.GroupPicture = groupPicture;
            dto.GroupUserTypes = groupUserTypes;
            dto.Remark = remark;
            dto.StorageStatus = storageStatus.ToInt32();
            var groupid = _groupInfoService.Save(dto);
            if (groupid.IsNullGuid())
            {
                return ToJsonResult(new { status = -1, msg = "保存出错" });
            }
            else
            {
                return ToJsonResult(new { status = 0, msg = "ok" });
            }
        }
        #endregion

        #region 查询组信息列表
        /// <summary>
        /// 查询组信息列表
        /// </summary>
        /// <param name="groupname">组名</param>
        /// <param name="page">页面索引</param>
        /// <param name="limit">页面大小</param>
        /// <returns></returns>
        public ActionResult Query(string groupname, int page = 1, int limit = 20)
        {
            int total = 0;
            var datalist = _groupInfoService.Query(page, limit, groupname, out total);

            datalist.ForEach(e =>
            {
                e.GroupPicture = e.GroupPicture ?? "";
                e.StorageStatus = _channelInfoService.FindStorageStatus(e.ChannelId);
            });
            return ToJsonResult(new { code = 0, data = datalist, count = total });
        }
        #endregion

        #region 查询用户
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="imUserName"></param>
        /// <param name="groupId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult QueryGroupUser(string imUserName, string groupId, string gender, int page = 1, int limit = 20)
        {
            var request = new QueryGroupUserRequest();
            request.NickName = imUserName;
            request.GroupId = groupId.ToGuid();
            request.Gender = (string.IsNullOrEmpty(gender) ? null : (int?)gender.ToInt32());
            request.PageIndex = page;
            request.PageSize = limit;

            var response = _groupInfoService.QueryGroupUser(request);

            #region 用户标识设置掩码
            if (response.IsSuccess && response.EntityList != null)
            {
                response.EntityList.ForEach(e =>
                {
                    e.UserHeadimg = string.IsNullOrEmpty(e.UserHeadimg) ? "../Content/Images/defaultimg.jpg" : e.UserHeadimg;
                });
            }
            #endregion

            return ToJsonResult(new { code = 0, groupId, data = response.EntityList, count = response.TotalCount });
        }
        #endregion

        #region 删除组用户
        /// <summary>
        /// 删除组用户
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public ActionResult DelGroupUser(string userGroupId)
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

        #region 删除组
        /// <summary>
        /// 删除组
        /// </summary>
        /// <returns></returns>
        public ActionResult Del(string groupId)
        {
            var res = _groupInfoService.Del(groupId.ToGuid());
            if (res)
            {
                return ToJsonResult(new { status = 0, msg = "ok" });
            }
            else
            {
                return ToJsonResult(new { status = 1, msg = "删除失败" });
            }
        }
        #endregion

        #region 保存用户组信息
        /// <summary>
        /// 保存用户组信息
        /// </summary>
        /// <remarks>
        /// 保存用户群昵称，角色类型
        /// </remarks>
        /// <returns></returns>
        public ActionResult SaveUserGroup(string userGroupId, string nickName, string roleType)
        {
            if (string.IsNullOrEmpty(userGroupId) || string.IsNullOrEmpty(nickName) || string.IsNullOrEmpty(roleType))
            {
                return ToJsonErrorResult(1, "参数不能为空");
            }

            var num = _userGroupService.SaveUserGroup(userGroupId.ToGuid(), nickName, roleType.ToInt32());

            if (num > 0)
            {
                return ToJsonResult(new { status = 0, msg = "ok" });
            }
            else
            {
                return ToJsonResult(new { status = -1, msg = "保存失败" });
            }
        }
        #endregion
    }
}