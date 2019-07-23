using System;
using System.Text.RegularExpressions;
using AutoMapper;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Model.Po;

namespace Mayiboy.IM.Logic.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region 默认映射关系

            //DateTime to String
            this.CreateMap<DateTime?, string>().ConvertUsing(e =>
            {
                if (!e.HasValue) return string.Empty;

                return e.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            });


            //String To DateTime

            this.CreateMap<string, DateTime?>().ConstructUsing(e =>
            {
                if (string.IsNullOrEmpty(e)) return null;

                if (e.Length == 8)
                {
                    if (Regex.IsMatch(e, "^[0-9]{4}[0-9]{2}[0-9]{2}$", RegexOptions.IgnoreCase))
                    {
                        return new DateTime(int.Parse(e.Substring(0, 4)), int.Parse(e.Substring(4, 2)), int.Parse(e.Substring(6, 2)));
                    }
                }

                return DateTime.Parse(e);
            });
            #endregion

            //用户信息
            this.CreateMap<ImUserInfoPo, ImUserInfoDto>();
            this.CreateMap<ImUserInfoDto, ImUserInfoPo>();

            //用户组信息
            this.CreateMap<UserGroupPo, UserGroupDto>();
            this.CreateMap<UserGroupDto, UserGroupPo>();

            //组信息
            this.CreateMap<GroupInfoPo, GroupInfoDto>();
            this.CreateMap<GroupInfoDto, GroupInfoPo>();

            //通道信息
            this.CreateMap<ChannelMessagePo, ChannelMessageDto>();
            this.CreateMap<ChannelMessageDto, ChannelMessagePo>();
        }
    }
}