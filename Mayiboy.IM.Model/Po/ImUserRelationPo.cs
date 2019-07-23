using System;
using SqlSugar;

namespace Mayiboy.IM.Model.Po
{
    /// <summary>
    /// 用户关系
    /// </summary>
    [SugarTable("ImUserRelation")]
    public class ImUserRelationPo
    {
        /// <summary>
        /// 用户关系标识
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid ImUserRelationId { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public Guid ImUserId { get; set; }

        /// <summary>
        /// 好友用户标识
        /// </summary>
        public Guid FgImUserId { get; set; }

        /// <summary>
        /// (0:待审核；1：申请通过；2：申请驳回)
        /// </summary>
        public int ApplyStatus { get; set; }

        /// <summary>
        /// 好友组标识
        /// </summary>
        public Guid FriendGroupId { get; set; }

        /// <summary>
        /// 通道标识
        /// </summary>
        public Guid ChannelId { get; set; }

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