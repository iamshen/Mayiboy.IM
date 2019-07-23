using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Mayiboy.Ioc;
using Mayiboy.IM.ConstDefine;
using Mayiboy.IM.Contract;
using Mayiboy.IM.DataAccess.Interface;
using Mayiboy.IM.Model.Po;
using Mayiboy.IM.Utils;
using Mayiboy.IM.Utils.Exception;

namespace Mayiboy.IM.Logic.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class UserGroupService : BaseService, IUserGroupService
    {
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IImUserInfoRepository _userInfoRepository;
        private readonly IGroupInfoRepository _groupInfoRepository;

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userGroupRepository"></param>
        /// <param name="userInfoRepository"></param>
        /// <param name="groupInfoRepository"></param>
        public UserGroupService(IUserGroupRepository userGroupRepository, IImUserInfoRepository userInfoRepository, IGroupInfoRepository groupInfoRepository)
        {
            _userGroupRepository = userGroupRepository;
            _userInfoRepository = userInfoRepository;
            _groupInfoRepository = groupInfoRepository;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryUserGroupResponse QueryUserGroup(QueryUserGroupRequest request)
        {
            var response = new QueryUserGroupResponse();

            return response;
        }

        #region 查询用户组信息
        /// <summary>
        /// 查询用户组信息
        /// </summary>
        /// <param name="imUserId">用户标识</param>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        public UserGroupDto FindUserGroup(Guid imUserId, Guid groupId)
        {
            var key = string.Concat(imUserId.ToString(), ":", groupId.ToString()).AddCachePrefix("tb:UserGroup");

            var usergroup = CacheManager.RedisDefault.Get(key,
                () => _userGroupRepository.Find<UserGroupPo>(e => e.ImUserId == imUserId && e.GroupId == groupId)?.As<UserGroupDto>(),
                PublicConst.Time.Minute30);

            return usergroup;
        }
        #endregion

        /// <summary>
        /// 查询用户组信息 根据用户标识、通道标识
        /// </summary>
        /// <param name="imUserId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public GroupInfoDto FindGroupInfo(Guid imUserId, Guid channelId)
        {
            var groupinfo = _groupInfoRepository.FindGroupInfo(imUserId, channelId);

            if (groupinfo != null)
            {
                return groupinfo.As<GroupInfoDto>();
            }

            return null;
        }

        #region 更新昵称
        /// <summary>
        /// 更新昵称
        /// </summary>
        /// <param name="imUid"></param>
        /// <param name="groupId"></param>
        /// <returns>昵称</returns>
        public string UdpateNickName(Guid imUid, Guid groupId)
        {
            var nickname = "";
            var res = _userInfoRepository.Find<ImUserInfoPo>(e => e.IsValid == 1 && e.ImUserId == imUid);
            if (res != null)
            {
                var entity = _userGroupRepository.Find<UserGroupPo>(e => e.IsValid == 1 && e.ImUserId == imUid && e.GroupId == groupId);
                if (entity != null && string.IsNullOrEmpty(entity.NickName))
                {
                    entity.NickName = res.ImUserName;
                    entity.UpdateTime = DateTime.Now;
                    if (_userGroupRepository.UpdateColumns(entity, e => new { e.NickName, e.UpdateTime }) > 0)
                    {
                        nickname = entity.NickName;
                    }

                    var key = string.Concat(imUid.ToString(), ":", groupId.ToString()).AddCachePrefix("tb:UserGroup");
                    CacheManager.RedisDefault.RemoveAsync(key);
                }
            }

            return nickname;
        }
        #endregion

        #region 查询所有用户
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        public List<ImUserInfoDto> QueryAllUser(Guid groupId)
        {
            List<ImUserInfoDto> list = new List<ImUserInfoDto>();
            var data = _userGroupRepository.QueryAllUser(groupId);
            if (data != null && data.Count > 0)
            {
                list = data.Select(e => e.As<ImUserInfoDto>()).ToList();
            }

            return list;
        }
        #endregion

        #region 保存用户组
        /// <summary>
        /// 保存用户组
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <param name="imUserId">用户标识</param>
        /// <param name="nickName">组内昵称</param>
        /// <param name="roleType">角色类型（0：普通用户；1：组长；2：管理员；）</param>
        /// <returns></returns>
        public Guid SaveUserGroup(Guid groupId, Guid imUserId, string nickName, int roleType)
        {
            //查询当前用户是否已经输入该组
            var entity = _userGroupRepository.Find<UserGroupPo>(e => e.IsValid == 1 && e.GroupId == groupId && e.ImUserId == imUserId);

            if (entity == null)
            {
                #region 如果组内昵称为空时使用用户名
                if (string.IsNullOrEmpty(nickName))
                {
                    var userinfoentity = _userInfoRepository.Find<ImUserInfoPo>(e => e.ImUserId == imUserId);
                    if (userinfoentity != null)
                    {
                        nickName = userinfoentity.ImUserName;
                    }
                }
                #endregion

                entity = new UserGroupPo()
                {
                    UserGroupId = Guid.NewGuid(),
                    GroupId = groupId,
                    ImUserId = imUserId,
                    NickName = nickName,
                    RoleType = roleType,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    IsValid = 1,
                };

                _userGroupRepository.Insert(entity);
            }
            else
            {
                //修改属性
                entity.NickName = nickName;
                entity.RoleType = roleType;
                entity.UpdateTime = DateTime.Now;
                _userGroupRepository.UpdateColumns(entity, e => new { e.NickName, e.RoleType, e.UpdateTime });
                var key = string.Concat(imUserId.ToString(), ":", groupId.ToString()).AddCachePrefix("tb:UserGroup");
                CacheManager.RedisDefault.Del(key);
            }

            return entity.UserGroupId;
        }
        #endregion

        #region 保存用户组
        /// <summary>
        /// 保存用户组
        /// </summary>
        /// <param name="imUserId">用户标识</param>
        /// <param name="groupIds">组标识集合</param>
        /// <param name="isremove">移除</param>
        /// <returns></returns>
        public int SaveUserGroup(Guid imUserId, List<Guid> groupIds, bool isremove)
        {
            var num = 0;
            if (isremove)
            {
                #region 移除组
                var listdata = _userGroupRepository.FindWhere<UserGroupPo>(e => e.IsValid == 1 && e.ImUserId == imUserId && groupIds.Contains(e.GroupId)).ToList();

                foreach (var item in listdata)
                {
                    item.UpdateTime = DateTime.Now;
                    item.IsValid = 0;
                    _userGroupRepository.UpdateColumns(item, e => new { e.UpdateTime, e.IsValid });
                    num++;
                }
                #endregion
            }
            else
            {
                #region 添加用户到组
                var userinfoentity = _userInfoRepository.Find<ImUserInfoPo>(e => e.IsValid == 1 && e.ImUserId == imUserId);

                if (userinfoentity == null)
                {
                    throw new LogicException("用户信息不存在");
                }

                foreach (var item in groupIds)
                {
                    var entity = new UserGroupPo()
                    {
                        UserGroupId = Guid.NewGuid(),
                        GroupId = item,
                        ImUserId = imUserId,
                        NickName = userinfoentity.ImUserName,
                        RoleType = 0,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        IsValid = 1
                    };

                    _userGroupRepository.Insert(entity);
                    num++;
                }
                #endregion
            }

            return num;
        }
        #endregion

        #region 删除用户组
        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="userGroupId">用户组</param>
        /// <returns></returns>
        public int Del(Guid userGroupId)
        {
            var entity = _userGroupRepository.Find<UserGroupPo>(e => e.IsValid == 1 && e.UserGroupId == userGroupId);

            if (entity == null)
            {
                throw new LogicException("删除用户组不存在");
            }

            entity.IsValid = 0;
            entity.UpdateTime = DateTime.Now;

            return _userGroupRepository.UpdateColumns(entity, e => new { e.IsValid, e.UpdateTime });
        }
        #endregion

        #region 修改组用户信息
        /// <summary>
        /// 修改组用户信息
        /// </summary>
        /// <param name="userGroupId">用户组标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="roletype">角色类型</param>
        /// <returns></returns>
        public int SaveUserGroup(Guid userGroupId, string nickName, int roletype)
        {
            var entity = _userGroupRepository.Find<UserGroupPo>(e => e.IsValid == 1 && e.UserGroupId == userGroupId);

            if (entity == null)
            {
                throw new LogicException("修改组用户信息不存在");
            }

            entity.NickName = nickName;
            entity.RoleType = roletype;
            entity.UpdateTime = DateTime.Now;

            return _userGroupRepository.UpdateColumns(entity, e => new { e.NickName, e.RoleType, e.UpdateTime });
        }
        #endregion
    }
}