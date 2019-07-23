using System;
using System.Collections.Generic;
using Framework.Mayiboy.Ioc;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IImUserInfoService : IBaseService, IDependency
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="imUserId">用户表示</param>
        /// <returns></returns>
        ImUserInfoDto Find(Guid imUserId);

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ImUserInfoDto Save(ImUserInfoDto dto);

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryImUserInfoResponse QueryImUserInfo(QueryImUserInfoRequest request);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="imuid"></param>
        /// <returns></returns>
        int Del(Guid imuid);

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        List<ImUserInfoDto> QueryAllUser(Guid groupId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryGroupResponse QueryGroup(QueryGroupRequest request);
    }
}