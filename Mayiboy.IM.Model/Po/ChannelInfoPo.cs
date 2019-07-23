﻿using System;
using SqlSugar;

namespace Mayiboy.IM.Model.Po
{
    /// <summary>
    /// 通道信息
    /// </summary>
    [SugarTable("ChannelInfo")]
    public class ChannelInfoPo
    {
        /// <summary>
        /// 通道标识
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid ChannelId { get; set; }

        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 存储（0：不存储；1：不存储）
        /// </summary>
        public int StorageStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否有效（0：无效；1：有效）
        /// </summary>
        public int IsValid { get; set; }
    }
}