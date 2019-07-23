using System;
using Mayiboy.IM.DataAccess.Interface;
using Mayiboy.IM.Model.Po;

namespace Mayiboy.IM.DataAccess.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelInfoRepository : BaseRepository, IChannelInfoRepository
    {
        /// <summary>
        /// 查询持久状态
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        public int FindStorageStatus(Guid channelId)
        {
            return CurrentDbContext.Queryable<ChannelInfoPo>().With(SqlSugar.SqlWith.NoLock)
                .Where(e => e.IsValid == 1 && e.ChannelId == channelId)
                .Select(e => e.StorageStatus).First();
        }

        /// <summary>
        /// 添加通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="storageStatus">存储（0：不存储；1：不存储）</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public Guid Add(string channelName, int storageStatus = 0, string remark = "")
        {
            var entity = new ChannelInfoPo();
            entity.ChannelId = Guid.NewGuid();
            entity.StorageStatus = storageStatus;
            entity.Remark = remark;
            entity.ChannelName = channelName;
            entity.UpdateTime = DateTime.Now;
            entity.CreateTime = DateTime.Now;
            entity.IsValid = 1;
            var num = CurrentDbContext.Insertable(entity).ExecuteCommand();
            if (num > 0)
            {
                return entity.ChannelId;
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}