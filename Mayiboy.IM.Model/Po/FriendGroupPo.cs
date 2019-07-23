using System;
using SqlSugar;

namespace Mayiboy.IM.Model.Po
{
    /// <summary>
    /// 好友组信息
    /// </summary>
    [SugarTable("ChannelMessage")]
    public class FriendGroupPo
    {
        /// <summary>
        /// 好友组标识
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid FriendGroupId { get; set; }

        /// <summary>
        /// 好友组名称
        /// </summary>
        public string FriendGroupName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否有效（0：无效；1：有效）
        /// </summary>
        public int IsValid { get; set; }
    }
}