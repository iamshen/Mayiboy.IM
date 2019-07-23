using System;
using System.Collections.Generic;
using Framework.Mayiboy.Ioc;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Model.Po;

namespace Mayiboy.IM.DataAccess.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGroupInfoRepository : IBaseRepository, IDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestImUserId">用户标识</param>
        /// <param name="requestGroupName">组名</param>
        /// <param name="requestPageIndex">页面索引</param>
        /// <param name="requestPageSize">页面大小</param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<GroupInfoDto> QueryGroup(Guid requestImUserId, string requestGroupName, int requestPageIndex, int requestPageSize, out int total);

        /// <summary>
        /// 查询用户组信息 根据用户标识、通道标识
        /// </summary>
        /// <param name="imUserId">用户标识</param>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        GroupInfoPo FindGroupInfo(Guid imUserId, Guid channelId);
    }
}