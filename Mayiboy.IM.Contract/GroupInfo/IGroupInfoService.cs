using System;
using System.Collections.Generic;
using Framework.Mayiboy.Ioc;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGroupInfoService : IBaseService, IDependency
    {

        /// <summary>
        /// 查询组信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        GroupInfoDto Find(Guid groupId);

        /// <summary>
        /// 保存组
        /// </summary>
        /// <param name="entity">组信息</param>
        /// <returns></returns>
        Guid Save(GroupInfoDto entity);

        /// <summary>
        /// 获取通道标识
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        string GetChannelId(Guid groupId);

        /// <summary>
        /// 查询组列表
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="groupname">组名</param>
        /// <param name="total">合计</param>
        /// <param name="pageindex">页面索引</param>
        /// <returns></returns>
        List<GroupInfoDto> Query(int pageindex, int pagesize, string groupname, out int total);

        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="groupInfoId"></param>
        /// <returns></returns>
        bool Del(Guid groupInfoId);

        /// <summary>
        /// 查询所有组信息
        /// </summary>
        /// <returns></returns>
        List<GroupInfoDto> QueryAll();

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        QueryGroupUserResponse QueryGroupUser(QueryGroupUserRequest request);
    }
}