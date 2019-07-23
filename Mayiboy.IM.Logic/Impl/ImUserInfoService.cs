using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.ConstDefine;
using Mayiboy.IM.Contract;
using Mayiboy.IM.DataAccess.Interface;
using Mayiboy.IM.Model.Po;
using Mayiboy.IM.Utils;
using Mayiboy.IM.Utils.Exception;

namespace Mayiboy.IM.Logic.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class ImUserInfoService : BaseService, IImUserInfoService
    {
        private readonly IImUserInfoRepository _imUserInfoRepository;
        private readonly IGroupInfoRepository _groupInfoRepository;

        #region ctor
        public ImUserInfoService(IImUserInfoRepository imUserInfoRepository, IGroupInfoRepository groupInfoRepository)
        {
            _imUserInfoRepository = imUserInfoRepository;
            _groupInfoRepository = groupInfoRepository;
        }
        #endregion

        /// <summary>
        /// 查找用户信息
        /// </summary>
        /// <param name="imUserId"></param>
        /// <returns></returns>
        public ImUserInfoDto Find(Guid imUserId)
        {
            string key = imUserId.ToString().AddCachePrefix("tb:UserInfo");

            var infuser = CacheManager.RedisDefault.Get(
                key,
                () => _imUserInfoRepository.FindSingle<ImUserInfoPo>(imUserId)?.As<ImUserInfoDto>(),
                PublicConst.Time.Minute30);

            return infuser;
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ImUserInfoDto Save(ImUserInfoDto dto)
        {
            if (dto.IsNull())
            {
                throw new LogicException("参数不能为空");
            }

            if (dto.ImUserId.IsNullGuid())
            {
                //新增
                var entity = dto.As<ImUserInfoPo>();
                entity.ImUserId = Guid.NewGuid();
                entity.CreateTime = DateTime.Now;
                entity.UpdateTime = DateTime.Now;
                entity.IsValid = 1;
                entity = _imUserInfoRepository.InsertReturnEntity(entity);
                dto = entity.As<ImUserInfoDto>();
            }
            else
            {
                #region 修改
                var entity = _imUserInfoRepository.Find<ImUserInfoPo>(e => e.IsValid == 1 && e.ImUserId == dto.ImUserId);
                if (entity.IsNull())
                {
                    throw new LogicException("修改用户不存在");
                }

                entity.ImUserName = dto.ImUserName;
                entity.UserHeadimg = dto.UserHeadimg;
                entity.Gender = dto.Gender;
                entity.UserId = dto.UserId;
                entity.UserType = dto.UserType;
                entity.Remark = dto.Remark;

                _imUserInfoRepository.UpdateColumns<ImUserInfoPo>(entity, e => new { e.ImUserName, e.UserHeadimg, e.UserId, e.UserType, e.Remark, e.Gender });
                #endregion

                string key = entity.ImUserId.ToString().AddCachePrefix("tb:UserInfo");
                CacheManager.RedisDefault.Del(key);
            }

            return dto;
        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryImUserInfoResponse QueryImUserInfo(QueryImUserInfoRequest request)
        {
            var response = new QueryImUserInfoResponse();

            if (request.IsNull())
            {
                throw new LogicException("参数不能为空");
            }

            var datalist = _imUserInfoRepository.QueryImUserInfo(request.ImUserName, request.UserId, request.UserType, request.GroupId, request.PageIndex, request.PageSize, out var total);

            if (datalist != null)
            {
                response.EntityList = datalist.Select(e => e.As<ImUserInfoDto>()).ToList();
                response.TotalCount = total;
            }

            return response;
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="imuid"></param>
        /// <returns></returns>
        public int Del(Guid imuid)
        {
            var entity = _imUserInfoRepository.Find<ImUserInfoPo>(e => e.ImUserId == imuid);
            if (entity == null)
            {
                throw new LogicException("删除用户信息不存在");
            }

            entity.UpdateTime = DateTime.Now;
            entity.IsValid = 0;
            return _imUserInfoRepository.UpdateColumns(entity, e => new { e.UpdateTime, e.IsValid });
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        public List<ImUserInfoDto> QueryAllUser(Guid groupId)
        {
            var datalist = _imUserInfoRepository.QueryAllUser(groupId);
            return datalist.Select(e => e.As<ImUserInfoDto>()).ToList();
        }

        /// <summary>
        /// 查询组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryGroupResponse QueryGroup(QueryGroupRequest request)
        {
            var response = new QueryGroupResponse();

            var datalist = _groupInfoRepository.QueryGroup(request.ImUserId, request.GroupName, request.PageIndex, request.PageSize, out var total);

            response.TotalCount = total;
            response.EntityList = datalist;
            return response;
        }
    }
}