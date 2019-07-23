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
    public interface IImUserInfoRepository : IBaseRepository, IDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestImUserName"></param>
        /// <param name="requestUserId"></param>
        /// <param name="requestUserType"></param>
        /// <param name="requestGroupId"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        List<ImUserInfoPo> QueryImUserInfo(string requestImUserName, string requestUserId, int? requestUserType, Guid? requestGroupId, int pageIndex, int pageSize, out int total);

        /// <summary>
        /// 查询组内所有的用户信息
        /// </summary>
        /// <param name="groupid">组标识</param>
        /// <returns></returns>
        List<ImUserInfoPo> QueryAllUser(Guid groupid);

        /// <summary>
        /// 查询用户组
        /// </summary>
        /// <param name="requestImUserId">用户标识</param>
        /// <param name="requestGroupName">组名</param>
        /// <param name="requestPageIndex">页面索引</param>
        /// <param name="requestPageSize">页面大小</param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<GroupInfoDto> QueryGroup(Guid requestImUserId, string requestGroupName, int requestPageIndex, int requestPageSize, out int total);
    }
}