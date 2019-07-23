using System;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 分页查询基类
    /// </summary>
    [Serializable]
    public class PageResponse : Response
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (TotalCount <= 0) return 0;

                return (int)Math.Ceiling(((decimal)TotalCount / (decimal)PageSize));
            }

        }
    }
}