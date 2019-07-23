using System;
using System.Collections.Generic;
using Mayiboy.IM.Contract;
using Mayiboy.IM.DataAccess.Interface;
using Mayiboy.IM.Model.Po;
using SqlSugar;

namespace Mayiboy.IM.DataAccess.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class UserGroupRepository : BaseRepository, IUserGroupRepository
    {

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        public List<ImUserInfoPo> QueryAllUser(Guid groupId)
        {
            #region sql script
            string sql = string.Format(@"SELECT top 100000
	                                        ui.ImUserId,
	                                        ISNULL(ug.NickName,ui.ImUserName) AS 'ImUserName',
	                                        ui.UserHeadimg,
	                                        ui.UserId,
	                                        ui.UserType,
	                                        ui.Remark,
	                                        ui.CreateTime,
	                                        ui.UpdateTime,
	                                        ui.IsValid
                                        FROM    
	                                        dbo.UserGroup ug WITH(NOLOCK)
                                            LEFT JOIN dbo.ImUserInfo ui WITH(NOLOCK) ON ug.ImUserId = ui.ImUserId
                                        WHERE   
	                                        ug.IsValid = 1
                                            AND ui.IsValid = 1
	                                        AND ug.GroupId='{0}'
                                        ORDER BY 
		                                        ug.CreateTime ASC", groupId.ToString());
            #endregion

            var data = CurrentDbContext.SqlQueryable<ImUserInfoPo>(sql).ToList();

            return data;
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="roletype">角色类型</param>
        /// <param name="total">合计</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        public List<ImUserInfoDto> QueryGroupUser(Guid groupId, string nickName, int? roletype, int pageIndex, int pageSize, out int total)
        {
            var totalcount = 0;

            #region 在组内查询用户信息
            var datalist = CurrentDbContext.Queryable<ImUserInfoPo, UserGroupPo>((t1, t2) => new object[]
                {
                    JoinType.Left, t1.ImUserId == t2.ImUserId
                })
                .Where((t1, t2) => t1.IsValid == 1 && t2.IsValid == 1 && t2.GroupId == groupId)
                .WhereIF(!string.IsNullOrEmpty(nickName), (t1, t2) => t2.NickName.Contains(nickName))
                .WhereIF(roletype.HasValue, (t1, t2) => t2.RoleType == roletype.Value)
                .With(SqlWith.NoLock)
                .OrderBy((t1, t2) => t2.CreateTime)
                .Select((t1, t2) => new ImUserInfoDto
                {
                    UserGroupId = t2.UserGroupId,
                    ImUserId = t1.ImUserId,
                    ImUserName = t2.NickName,
                    UserHeadimg = t1.UserHeadimg,
                    Gender = t1.Gender,
                    UserId = t1.UserId,
                    UserType = t1.UserType,
                    Remark = t1.Remark,
                    RoleType = t2.RoleType
                })
                .ToPageList(pageIndex, pageSize, ref totalcount);
            #endregion

            total = totalcount;
            return datalist;
        }
    }
}