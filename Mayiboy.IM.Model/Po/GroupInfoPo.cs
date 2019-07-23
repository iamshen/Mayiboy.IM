﻿using System;
using SqlSugar;

namespace Mayiboy.IM.Model.Po
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("GroupInfo")]
    public class GroupInfoPo
    {
        /// <summary>
        /// 组标识
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid GroupId { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 组内最大容量
        /// </summary>
        public int? GroupMaxCapacity { get; set; }

        /// <summary>
        /// 组图片
        /// </summary>
        public string GroupPicture { get; set; }

        /// <summary>
        /// 组内用户类型(1,2,3)
        /// </summary>
        public string GroupUserTypes { get; set; }

        /// <summary>
        /// 通道标识
        /// </summary>
        public Guid ChannelId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否有效（0：无效；1：有效）
        /// </summary>
        public int IsValid { get; set; }
    }
}