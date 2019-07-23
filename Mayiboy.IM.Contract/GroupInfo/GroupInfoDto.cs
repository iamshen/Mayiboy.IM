using System;

namespace Mayiboy.IM.Contract
{
    public class GroupInfoDto
    {
        /// <summary>
        /// 组标识
        /// </summary>
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
        /// 组内用户类型
        /// </summary>
        public string GroupUserTypes { get; set; }

        /// <summary>
        /// 通道标识
        /// </summary>
        public Guid ChannelId { get; set; }

        /// <summary>
        /// 存储（0：不存储；1：不存储）
        /// </summary>
        public int StorageStatus { get; set; }

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

        /// <summary>
        /// 用户组标识
        /// </summary>
        public Guid? UserGroupId { get; set; }
    }
}