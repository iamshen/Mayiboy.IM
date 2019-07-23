using System;

namespace Mayiboy.IM.Admin.UI.Models
{
    public class MenuInfo
    {
        /// <summary>
        /// 菜单标示
        /// </summary>
        public Guid Id => Guid.NewGuid();

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        public string Url { get; set; }
    }
}