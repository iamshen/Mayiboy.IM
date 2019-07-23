using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Host.Models;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.Host.Controllers
{
    public class ImUserInfoController : BaseApiController
    {
        private readonly IImUserInfoService _imUserInfoService;

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imUserInfoService"></param>
        public ImUserInfoController(IImUserInfoService imUserInfoService)
        {
            _imUserInfoService = imUserInfoService;
        }
        #endregion

        #region 用户信息
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<InfoResponse> Info(InfoRequest request)
        {
            var response = new InfoResponse();
            var entity = _imUserInfoService.Find(request.ImUserId.ToGuid());
            if (entity == null)
            {
                return Error<InfoResponse>("1", "用户不存在");
            }
            response.ImUserId = entity.ImUserId;
            response.ImUserName = entity.ImUserName;
            response.UserHeadimg = entity.UserHeadimg;
            response.Gender = entity.Gender;
            response.UserId = entity.UserId;
            response.UserType = entity.UserType;
            response.Remark = entity.Remark;
            return Success(response);
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="imUserId">用户标识</param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<InfoResponse> Info(string imUserId)
        {
            var response = new InfoResponse();
            var entity = _imUserInfoService.Find(imUserId.ToGuid());
            if (entity == null)
            {
                return Error<InfoResponse>("1", "用户不存在");
            }
            response.ImUserId = entity.ImUserId;
            response.ImUserName = entity.ImUserName;
            response.UserHeadimg = entity.UserHeadimg;
            response.Gender = entity.Gender;
            response.UserId = entity.UserId;
            response.UserType = entity.UserType;
            response.Remark = entity.Remark;
            return Success(response);
        }
        #endregion

        #region 添加临时用户
        /// <summary>
        /// 添加临时用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<AddCasualResponse> AddCasual(AddCasualRequest request)
        {
            var response = new AddCasualResponse();

            var dto = new ImUserInfoDto();
            dto.ImUserName = request.ImUserName;
            //TODO:处理临时用户头像
            dto.UserHeadimg = "";
            dto.UserType = 4;
            dto = _imUserInfoService.Save(dto);
            response.ImUserId = dto.ImUserId;
            response.ImUserName = dto.ImUserName;
            response.UserHeadimg = dto.UserHeadimg;
            return Success(response);
        }

        /// <summary>
        /// 添加临时用户
        /// </summary>
        /// <param name="imUserName">用户名</param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<AddCasualResponse> AddCasual(string imUserName)
        {
            var response = new AddCasualResponse();

            var dto = new ImUserInfoDto();
            dto.ImUserName = imUserName;

            #region 设置默认头像
            //var bytes = HeadimgHelper.Generate(imUserName.Substring(0, 1));
            //var filename = Guid.NewGuid().ToString() + ".jpg";
            //var fileres = UploadFile.Push(bytes, "imheadimg", filename, filename);
            //if (fileres.Status == "success")
            //{
            //    dto.UserHeadimg = fileres.Url;
            //}
            #endregion

            dto.UserType = 4;
            dto = _imUserInfoService.Save(dto);
            response.ImUserId = dto.ImUserId;
            response.ImUserName = dto.ImUserName;
            response.UserHeadimg = dto.UserHeadimg;
            return Success(response);
        }
        #endregion
    }
}
