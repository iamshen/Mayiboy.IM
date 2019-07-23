using System;
using System.Collections.Generic;
using Framework.Mayiboy.Ioc;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserGroupService : IBaseService, IDependency
    {
        /// <summary>
        /// 查询用户组信息
        /// </summary>
        /// <param name="imUserId">用户标识</param>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        UserGroupDto FindUserGroup(Guid imUserId, Guid groupId);

        /// <summary>
        /// 查询用户组信息 根据用户标识、通道标识
        /// </summary>
        /// <param name="imUserId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        GroupInfoDto FindGroupInfo(Guid imUserId, Guid channelId);

        /// <summary>
        /// 更新昵称
        /// </summary>
        /// <param name="imUid"></param>
        /// <param name="groupId"></param>
        string UdpateNickName(Guid imUid, Guid groupId);

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        List<ImUserInfoDto> QueryAllUser(Guid groupId);

        /// <summary>
        /// 保存用户组
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <param name="imUserId">用户标识</param>
        /// <param name="nickName">组内昵称</param>
        /// <param name="roleType">角色类型（0：普通用户；1：组长；2：管理员；）</param>
        /// <returns></returns>
        Guid SaveUserGroup(Guid groupId, Guid imUserId, string nickName, int roleType);

        /// <summary>
        /// 保存用户组
        /// </summary>
        /// <param name="imUserId">用户标识</param>
        /// <param name="groupIds">组标识集合</param>
        /// <param name="isremove">移除</param>
        /// <returns></returns>
        int SaveUserGroup(Guid imUserId, List<Guid> groupIds, bool isremove);

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="userGroupId">用户组标识</param>
        /// <returns></returns>
        int Del(Guid userGroupId);

        /// <summary>
        /// 保存用户组信息
        /// </summary>
        /// <param name="userGroupId">用户标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="roletype">角色类型</param>
        /// <returns></returns>
        int SaveUserGroup(Guid userGroupId, string nickName, int roletype);
    }
}