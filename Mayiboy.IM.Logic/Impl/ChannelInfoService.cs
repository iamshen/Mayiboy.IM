using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.ConstDefine;
using Mayiboy.IM.Contract;
using Mayiboy.IM.DataAccess.Interface;
using Mayiboy.IM.Model.Po;
using Mayiboy.IM.Utils;
using Mayiboy.IM.Utils.Exception;

namespace Mayiboy.IM.Logic.Impl
{
    public class ChannelInfoService : BaseService, IChannelInfoService
    {
        private readonly IChannelInfoRepository _channelInfoRepository;

        public ChannelInfoService(IChannelInfoRepository channelInfoRepository)
        {
            _channelInfoRepository = channelInfoRepository;
        }

        /// <summary>
        /// 查询持久状态
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        public int FindStorageStatus(Guid channelId)
        {
            string key = channelId.ToString().AddCachePrefix("StorageStatus");
            var storageStatus = CacheManager.RedisDefault.Get(key, () => _channelInfoRepository.FindStorageStatus(channelId).ToString(), PublicConst.Time.Hour4);
            return storageStatus.ToInt32();
        }

        /// <summary>
        /// 查询通道信息
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="pageindex">页面索引</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ChannelInfoDto> Query(string channelName, int pageindex, int pagesize, out int total)
        {
            var list = new List<ChannelInfoDto>();
            total = 0;

            var data = _channelInfoRepository.FindPage<ChannelInfoPo>(e => e.IsValid == 1
                 && (SqlSugar.SqlFunc.IsNullOrEmpty(channelName) || e.ChannelName.Contains(channelName)),
                o => o.CreateTime, pageindex, pagesize, ref total);

            if (data != null)
            {
                list = data.Select(e => e.As<ChannelInfoDto>()).ToList();
            }

            return list;
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
            return _channelInfoRepository.Add(channelName, storageStatus, remark);
        }

        /// <summary>
        /// 更新通道信息
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <param name="channelName">通道名称</param>
        /// <param name="storageStatus">存在状态</param>
        /// <param name="remark">备注</param>
        public void Update(Guid channelId, string channelName, int storageStatus, string remark)
        {
            var entity = _channelInfoRepository.Find<ChannelInfoPo>(e => e.IsValid == 1 && e.ChannelId == channelId);
            if (entity == null)
            {
                throw new LogicException("更新通道不存在");
            }
            entity.ChannelName = channelName;
            entity.StorageStatus = storageStatus;
            entity.UpdateTime = DateTime.Now;
            entity.Remark = remark;

            var rownum = _channelInfoRepository.UpdateColumns(entity, e => new { e.ChannelName, e.StorageStatus, e.Remark, e.UpdateTime });
            if (rownum > 0)
            {
                string key = channelId.ToString().AddCachePrefix("StorageStatus");
                CacheManager.RedisDefault.Del(key);
            }
        }

        /// <summary>
        /// 删除通道信息
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        public int Del(Guid channelId)
        {
            var entity = _channelInfoRepository.Find<ChannelInfoPo>(e => e.IsValid == 1 && e.ChannelId == channelId);
            if (entity == null)
            {
                throw new LogicException("删除通道信息不存在");
            }
            entity.UpdateTime = DateTime.Now;
            entity.IsValid = 0;

            return _channelInfoRepository.UpdateColumns(entity, e => new { e.IsValid, e.UpdateTime });
        }
    }
}