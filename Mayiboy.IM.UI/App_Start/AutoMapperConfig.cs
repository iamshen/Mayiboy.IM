using AutoMapper;
using Mayiboy.IM.Logic.Mapper;

namespace Mayiboy.IM.UI
{
    public class AutoMapperConfig
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            Mapper.Initialize(e =>
            {
                AddMapper(e);
                e.AddProfile(new MapperProfile());
            });
        }

        /// <summary>
        /// 添加映射映射关系
        /// </summary>
        /// <param name="config"></param>
        private static void AddMapper(IMapperConfigurationExpression config)
        {
            //config.CreateMap<GroupUserModel, GroupInfoDto>();
            //config.CreateMap<GroupInfoDto, GroupUserModel>();
        }
    }
}