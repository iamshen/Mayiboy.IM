using System;

namespace Mayiboy.IM.Admin.UI.Models
{
    public class GroupUserModel
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

        /// <summary>
        /// 
        /// </summary>
        public bool LAY_CHECKED
        {
            get
            {
                if (UserGroupId.HasValue)
                {
                    return UserGroupId.Value != Guid.Empty;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}