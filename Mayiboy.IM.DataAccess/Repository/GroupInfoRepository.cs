using System;
using System.Collections.Generic;
using Mayiboy.IM.Contract;
using Mayiboy.IM.DataAccess.Interface;
using Mayiboy.IM.Model.Po;

namespace Mayiboy.IM.DataAccess.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupInfoRepository : BaseRepository, IGroupInfoRepository
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
        public List<GroupInfoDto> QueryGroup(Guid requestImUserId, string requestGroupName, int requestPageIndex, int requestPageSize, out int total)
        {
            List<GroupInfoDto> datalist = new List<GroupInfoDto>();
            total = 0;
            if (requestImUserId != Guid.Empty)
            {
                //查询用户类型
                var usertype = CurrentDbContext.Queryable<ImUserInfoPo>().With(SqlSugar.SqlWith.NoLock).Where(e => e.IsValid == 1 && e.ImUserId == requestImUserId).Select(e => e.UserType).First();

                datalist = CurrentDbContext.Queryable<GroupInfoPo>().With(SqlSugar.SqlWith.NoLock)
                    .Where(e => e.IsValid == 1 && e.GroupUserTypes.Contains(usertype.ToString()))
                    .WhereIF(!string.IsNullOrEmpty(requestGroupName), e => e.GroupName.Contains(requestGroupName))
                    .WhereIF(usertype == 0, e => false)
                    .Select(e => new GroupInfoDto
                    {
                        GroupId = e.GroupId,
                        GroupName = e.GroupName,
                        GroupMaxCapacity = e.GroupMaxCapacity,
                        GroupPicture = e.GroupPicture,
                        GroupUserTypes = e.GroupUserTypes,
                        Remark = e.Remark,
                        CreateTime = e.CreateTime,
                        UpdateTime = e.UpdateTime,
                        UserGroupId = SqlSugar.SqlFunc.Subqueryable<UserGroupPo>()
                            .Where(o => o.IsValid == 1 && o.GroupId == e.GroupId && o.ImUserId == requestImUserId)
                            .Select(o => o.UserGroupId)
                    }).ToPageList(requestPageIndex, requestPageSize, ref total);
            }

            return datalist;
        }

        /// <summary>
        /// 查询用户组信息 根据用户标识、通道标识
        /// </summary>
        /// <param name="imUserId">用户标识</param>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        public GroupInfoPo FindGroupInfo(Guid imUserId, Guid channelId)
        {
            var entity = CurrentDbContext.Queryable<GroupInfoPo, UserGroupPo>((t1, t2) => new object[]
                 {
                    SqlSugar.JoinType.Left, t1.GroupId == t2.GroupId
                 }).With(SqlSugar.SqlWith.NoLock)
                .Where((t1, t2) => t1.IsValid == 1 && t2.IsValid == 1 && t2.ImUserId == imUserId && t1.ChannelId == channelId).First();

            return entity;
        }
    }
}