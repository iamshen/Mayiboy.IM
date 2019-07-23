using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Mayiboy.Ioc;
using Mayiboy.IM.ConstDefine;
using Mayiboy.IM.Contract;
using Mayiboy.IM.DataAccess.Interface;
using Mayiboy.IM.Model.Po;
using Mayiboy.IM.Utils;
using Mayiboy.IM.Utils.Exception;
using SqlSugar;

namespace Mayiboy.IM.Logic.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupInfoService : BaseService, IGroupInfoService
    {
        private readonly IGroupInfoRepository _groupInfoRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IChannelInfoRepository _channelInfoRepository;

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupInfoRepository"></param>
        /// <param name="userGroupRepository"></param>
        /// <param name="channelInfoRepository"></param>
        public GroupInfoService(IGroupInfoRepository groupInfoRepository, IUserGroupRepository userGroupRepository, IChannelInfoRepository channelInfoRepository)
        {
            _groupInfoRepository = groupInfoRepository;
            _userGroupRepository = userGroupRepository;
            _channelInfoRepository = channelInfoRepository;
        }
        #endregion

        /// <summary>
        /// 查询组信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public GroupInfoDto Find(Guid groupId)
        {
            var entity = _groupInfoRepository.Find<GroupInfoPo>(e => e.IsValid == 1 && e.GroupId == groupId);

            if (entity != null)
            {
                return entity.As<GroupInfoDto>();
            }

            return null;
        }

        /// <summary>
        /// 获取通道标识
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        public string GetChannelId(Guid groupId)
        {
            var channelId = CacheManager.RunTimeCache.Get(groupId.ToString(), () =>
            {
                string key = groupId.ToString().AddCachePrefix("tb:GroupInfo");
                var groupInfo = CacheManager.RedisDefault.Get(key, () =>
                {
                    var groupinfo = _groupInfoRepository.Find<GroupInfoPo>(e => e.IsValid == 1 && e.GroupId == groupId);
                    if (groupinfo != null)
                    {
                        return groupinfo.As<GroupInfoDto>();
                    }
                    return null;
                }, PublicConst.Time.Hour4);

                if (groupInfo != null)
                {
                    return groupInfo.ChannelId.ToString();
                }
                else
                {
                    return "";
                }
            }, PublicConst.Time.Hour4);

            return channelId;
        }

        /// <summary>
        /// 保存组
        /// </summary>
        /// <param name="entity">组信息</param>
        /// <returns></returns>
        public Guid Save(GroupInfoDto entity)
        {
            var guid = Guid.Empty;
            if (entity.GroupId == Guid.Empty)
            {
                #region 新增
                var channelId = _channelInfoRepository.Add(entity.GroupName + "_channel", entity.StorageStatus, "用户组消息通道");//新增通道
                var data = entity.As<GroupInfoPo>();
                data.GroupId = Guid.NewGuid();
                data.CreateTime = DateTime.Now;
                data.UpdateTime = DateTime.Now;
                data.ChannelId = channelId;
                data.IsValid = 1;
                if (_groupInfoRepository.Insert(data) > 0)
                {
                    guid = data.GroupId;
                }
                #endregion
            }
            else
            {
                #region 修改
                var data = _groupInfoRepository.Find<GroupInfoPo>(e => e.IsValid == 1 && e.GroupId == entity.GroupId);

                if (data == null)
                {
                    throw new LogicException(-1, "修改组信息不存在！");
                }

                data.GroupName = entity.GroupName;
                data.GroupMaxCapacity = entity.GroupMaxCapacity;
                data.GroupPicture = entity.GroupPicture;
                data.GroupUserTypes = entity.GroupUserTypes;
                data.Remark = entity.Remark;
                data.UpdateTime = DateTime.Now;

                var rownumber = _groupInfoRepository.UpdateColumns(data, e => new
                {
                    e.GroupName,
                    e.GroupMaxCapacity,
                    e.GroupPicture,
                    e.GroupUserTypes,
                    e.Remark,
                    e.UpdateTime
                });
                if (rownumber > 0)
                {
                    guid = data.GroupId;
                }

                //更新通道信息存储状态
                var channel = _channelInfoRepository.Find<ChannelInfoPo>(e => e.IsValid == 1 && e.ChannelId == data.ChannelId);
                if (channel != null)
                {
                    channel.StorageStatus = entity.StorageStatus;
                    channel.UpdateTime = DateTime.Now;
                    _channelInfoRepository.UpdateColumns(channel, e => new { e.StorageStatus, e.UpdateTime });

                    string key = entity.ChannelId.ToString().AddCachePrefix("StorageStatus");
                    CacheManager.RedisDefault.Remove(key);
                }
                #endregion
            }

            return guid;
        }

        /// <summary>
        /// 查询组列表
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="groupname">组名</param>
        /// <param name="total">合计</param>
        /// <param name="pageindex">页面索引</param>
        /// <returns></returns>
        public List<GroupInfoDto> Query(int pageindex, int pagesize, string groupname, out int total)
        {
            var list = new List<GroupInfoDto>();
            total = 0;

            var data = _groupInfoRepository.FindPage<GroupInfoPo>(
                e => e.IsValid == 1 && (SqlFunc.IsNullOrEmpty(groupname) || e.GroupName.Contains(groupname)),
                o => o.CreateTime, pageindex, pagesize, ref total);

            if (data != null)
            {
                list = data.Select(e => e.As<GroupInfoDto>()).ToList();
            }
            return list;
        }

        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="groupInfoId"></param>
        /// <returns></returns>
        public bool Del(Guid groupInfoId)
        {
            var data = _groupInfoRepository.Find<GroupInfoPo>(e => e.IsValid == 1 && e.GroupId == groupInfoId);

            if (data == null)
            {
                throw new LogicException(-1, "删除组不存在");
            }

            data.IsValid = 0;
            data.UpdateTime = DateTime.Now;

            return _groupInfoRepository.UpdateColumns(data, e => new { e.IsValid, e.UpdateTime }) > 0;
        }

        /// <summary>
        /// 查询所有组信息
        /// </summary>
        /// <returns></returns>
        public List<GroupInfoDto> QueryAll()
        {
            var list = new List<GroupInfoDto>();
            var data = _groupInfoRepository.FindWhere<GroupInfoPo>(e => e.IsValid == 1);

            if (data != null)
            {
                list = data.Select(e => e.As<GroupInfoDto>()).ToList();
            }

            return list;
        }

        /// <summary>
        /// 查询群组内用户信息
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public QueryGroupUserResponse QueryGroupUser(QueryGroupUserRequest request)
        {
            var response = new QueryGroupUserResponse();
            var datalist = _userGroupRepository.QueryGroupUser(request.GroupId, request.NickName, request.RoleUser, request.PageIndex, request.PageSize, out var total);
            response.EntityList = datalist;
            response.TotalCount = total;
            return response;
        }
    }
}