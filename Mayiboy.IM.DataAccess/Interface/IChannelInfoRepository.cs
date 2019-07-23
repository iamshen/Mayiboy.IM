using System;
using Framework.Mayiboy.Ioc;

namespace Mayiboy.IM.DataAccess.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChannelInfoRepository : IBaseRepository, IDependency
    {
        /// <summary>
        /// 查询持久状态
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        int FindStorageStatus(Guid channelId);

        /// <summary>
        /// 添加通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="storageStatus">存储（0：不存储；1：不存储）</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        Guid Add(string channelName, int storageStatus = 0, string remark = "");
    }
}