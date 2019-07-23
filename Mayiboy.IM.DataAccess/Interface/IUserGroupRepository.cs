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
    public interface IUserGroupRepository : IBaseRepository, IDependency
    {
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        List<ImUserInfoPo> QueryAllUser(Guid groupId);

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="roletype">角色类型</param>
        /// <param name="pageSize">也没大小</param>
        /// <param name="total">合计</param>
        /// <param name="pageIndex">页面索引</param>
        /// <returns></returns>
        List<ImUserInfoDto> QueryGroupUser(Guid groupId, string nickName, int? roletype, int pageIndex, int pageSize, out int total);
    }
}