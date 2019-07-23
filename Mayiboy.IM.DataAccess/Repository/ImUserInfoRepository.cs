using System;
using System.Collections.Generic;
using Mayiboy.IM.Contract;
using Mayiboy.IM.DataAccess.Interface;
using Mayiboy.IM.Model.Po;
using Mayiboy.IM.Utils;
using SqlSugar;

namespace Mayiboy.IM.DataAccess.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class ImUserInfoRepository : BaseRepository, IImUserInfoRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestImUserName">用户名</param>
        /// <param name="requestUserId">用户标示</param>
        /// <param name="requestUserType">用户类型(1:内部用户，2：会员用户；3：第三方用户；4：临时用户)</param>
        /// <param name="requestGroupId">组标识</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="total">合计</param>
        /// <param name="pageIndex">页面索引</param>
        /// <returns></returns>
        public List<ImUserInfoPo> QueryImUserInfo(string requestImUserName, string requestUserId, int? requestUserType, Guid? requestGroupId, int pageIndex, int pageSize, out int total)
        {
            List<ImUserInfoPo> list = new List<ImUserInfoPo>();
            var totalcount = 0;
            if (requestGroupId == null)
            {
                #region 根据用户信息查询
                var datalist = CurrentDbContext.Queryable<ImUserInfoPo>().With(SqlWith.NoLock).Where(e =>
                                 e.IsValid == 1
                                 && (SqlFunc.IsNullOrEmpty(requestImUserName) || e.ImUserName.Contains(requestImUserName))
                                 && (SqlFunc.IsNullOrEmpty(requestUserId) || e.UserId == requestUserId)
                                 && (SqlFunc.IsNullOrEmpty(requestUserType) || e.UserType == requestUserType))
                    .OrderBy(e => e.CreateTime)
                            .ToPageList(pageIndex, pageSize, ref totalcount);

                if (datalist != null)
                {
                    list = datalist;
                }
                #endregion
            }
            else
            {
                #region 在组内查询用户信息
                var datalist = CurrentDbContext.Queryable<ImUserInfoPo, UserGroupPo>((t1, t2) => new object[]
                       {
                    JoinType.Left, t1.ImUserId == t2.ImUserId
                       }).Where((t1, t2) =>
                           t2.GroupId == requestGroupId.Value
                           && t1.IsValid == 1 && t2.IsValid == 1
                           && (SqlFunc.IsNullOrEmpty(requestImUserName) || t1.ImUserName.Contains(requestImUserName))
                           && (SqlFunc.IsNullOrEmpty(requestUserId) || t1.UserId == requestUserId)
                           && (SqlFunc.IsNullOrEmpty(requestUserType) || t1.UserType == requestUserType)
                        ).With(SqlWith.NoLock).OrderBy((t1, t2) => t2.CreateTime).Select((t1, t2) => t1).ToPageList(pageIndex, pageSize, ref totalcount);
                #endregion

                if (datalist != null)
                {
                    list = datalist;
                }
            }

            total = totalcount;
            return list;
        }

        /// <summary>
        /// 查询组内所有的用户信息
        /// </summary>
        /// <param name="groupid">组标识</param>
        /// <returns></returns>
        public List<ImUserInfoPo> QueryAllUser(Guid groupid)
        {
            var list = new List<ImUserInfoPo>();

            if (!groupid.IsNullGuid())
            {
                list = CurrentDbContext.Queryable<ImUserInfoPo, UserGroupPo>((t1, t2) => new object[]
                 {
                    JoinType.Left, t1.ImUserId == t2.ImUserId
                 }).Where((t1, t2) => t2.GroupId == groupid && t2.IsValid == 1
                ).With(SqlWith.NoLock).OrderBy((t1, t2) => t2.CreateTime).Select((t1, t2) => t1).ToList();
            }

            return list;
        }

        /// <summary>
        /// 查询用户组列表
        /// </summary>
        /// <param name="requestImUserId">用户标识</param>
        /// <param name="requestGroupName">用户组名</param>
        /// <param name="requestPageIndex">页面索引</param>
        /// <param name="requestPageSize">也没大小</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<GroupInfoDto> QueryGroup(Guid requestImUserId, string requestGroupName, int requestPageIndex, int requestPageSize, out int total)
        {
            total = 0;
            var datalist = CurrentDbContext.Queryable<UserGroupPo, GroupInfoPo>((t1, t2) => new object[]
                {
                    JoinType.Left, t1.GroupId == t2.GroupId
                }).With(SqlWith.NoLock)
                .Where((t1, t2) => t1.IsValid == 1)
                .WhereIF(!string.IsNullOrEmpty(requestGroupName), (t1, t2) => t2.GroupName.Contains(requestGroupName))
                .Select((t1, t2) => new GroupInfoDto
                {
                    GroupId = t2.GroupId,
                    GroupName = t2.GroupName,
                    GroupMaxCapacity = t2.GroupMaxCapacity,
                    GroupPicture = t2.GroupPicture,
                    GroupUserTypes = t2.GroupUserTypes,
                    Remark = t2.Remark,
                    CreateTime = t2.CreateTime,
                    UpdateTime = t2.UpdateTime,
                    UserGroupId = t1.UserGroupId
                }).ToPageList(requestPageIndex, requestPageSize, ref total);

            return datalist;
        }
    }
}