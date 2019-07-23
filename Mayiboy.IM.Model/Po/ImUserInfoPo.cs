using System;
using SqlSugar;

namespace Mayiboy.IM.Model.Po
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("ImUserInfo")]
    public class ImUserInfoPo
    {

        /// <summary>
        /// 即时通讯用户标识
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid ImUserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string ImUserName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserHeadimg { get; set; }

        /// <summary>
        /// 性别（0：女；1：男）
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户类型(1:内部用户，2：会员用户；3：第三方用户；4：临时用户)
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; set; }

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