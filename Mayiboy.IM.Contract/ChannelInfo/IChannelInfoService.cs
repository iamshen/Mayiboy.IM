using System;
using System.Collections.Generic;
using Framework.Mayiboy.Ioc;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChannelInfoService : IBaseService, IDependency
    {

        /// <summary>
        /// 查询持久状态
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        int FindStorageStatus(Guid channelId);

        /// <summary>
        /// 查询通道信息
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="pageindex">页面索引</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<ChannelInfoDto> Query(string channelName, int pageindex, int pagesize, out int total);

        /// <summary>
        /// 添加通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="storageStatus">存储（0：不存储；1：不存储）</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        Guid Add(string channelName, int storageStatus = 0, string remark = "");

        /// <summary>
        /// 更新通道信息
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <param name="channelName">通道名称</param>
        /// <param name="storageStatus">存在状态</param>
        /// <param name="remark">备注</param>
        void Update(Guid channelId, string channelName, int storageStatus, string remark);

        /// <summary>
        /// 删除通道信息
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        int Del(Guid channelId);
    }
}