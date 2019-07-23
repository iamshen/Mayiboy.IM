using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.Utils.Exception;

namespace Mayiboy.IM.Admin.UI.Exception
{
    /// <summary>
    /// 自定义Mvc异常基类
    /// </summary>
    public class MvcBaseException : BaseException
    {
        public MvcBaseException(int code, string message) : base(code, message)
        {
        }

        public MvcBaseException(string message) : base(message)
        {
        }

        public MvcBaseException(object data) : base(data)
        {
        }

        /// <summary>
        /// Ajax处理函数
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void AjaxHandler(ExceptionContext filterContext)
        {
            var result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            if (ResultData == null)
            {
                result.Data = new
                {
                    status = ExceptionCode,
                    msg = ExceptionMessage
                };
            }
            else
            {
                result.Data = ResultData;
            }

            filterContext.Result = result;
        }

        /// <summary>
        /// 视图处理函数
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void ViewHandler(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect(PublicConstConfig.Url.SystemException);
        }

        /// <summary>
        /// 异常结果处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected void ExceptionResultHandler(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                AjaxHandler(filterContext);
            }
            else
            {
                ViewHandler(filterContext);
            }
        }

        /// <summary>
        /// 注册自定义定义异常处理
        /// </summary>
        /// <param name="filterContext"></param>
        public static void RegisterExceptionHandler(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception as MvcBaseException;
            if (exception != null)
            {
                exception.ExceptionResultHandler(filterContext);
                filterContext.ExceptionHandled = true;
            }

            //处理其他异常
            ExceptionHandler(filterContext);
        }

        /// <summary>
        /// 处理其他自定义异常
        /// </summary>
        /// <param name="filterContext"></param>
        protected static void ExceptionHandler(ExceptionContext filterContext)
        {
            if ((filterContext.Exception as DataAccessException) != null)
            {
                var dataAccessException = filterContext.Exception as DataAccessException;

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    #region dataAccessException
                    var result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                    if (dataAccessException.ResultData == null)
                    {
                        result.Data = new
                        {
                            status = dataAccessException.ExceptionCode,
                            msg = dataAccessException.ExceptionMessage
                        };
                    }
                    else
                    {
                        result.Data = dataAccessException.ResultData;
                    }

                    filterContext.Result = result;
                    #endregion
                }
                else
                {
                    #region dataAccessException
                    ContentResult content = new ContentResult();

                    if (dataAccessException.ResultData == null)
                    {
                        content.Content = string.Concat("[",
                            dataAccessException.ExceptionCode,
                            "]",
                            dataAccessException.Message);
                    }
                    else
                    {
                        content.Content = dataAccessException.ResultData.ToJson();
                    }
                    #endregion
                    filterContext.Result = content;
                }

                //TODO:是否记录日志 后续开发

                filterContext.ExceptionHandled = true;
            }
            else if ((filterContext.Exception as LogicException) != null)
            {
                var logicException = filterContext.Exception as LogicException;
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    #region logicException
                    var result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                    if (logicException.ResultData == null)
                    {
                        result.Data = new
                        {
                            status = logicException.ExceptionCode,
                            msg = logicException.ExceptionMessage
                        };
                    }
                    else
                    {
                        result.Data = logicException.ResultData;
                    }

                    filterContext.Result = result;
                    #endregion
                }
                else
                {
                    #region logicException
                    ContentResult content = new ContentResult();

                    if (logicException.ResultData == null)
                    {
                        content.Content = string.Concat("[",
                            logicException.ExceptionCode,
                            "]",
                            logicException.Message);
                    }
                    else
                    {
                        content.Content = logicException.ResultData.ToJson();
                    }
                    #endregion
                    filterContext.Result = content;
                }

                //TODO:是否记录日志 后续开发

                filterContext.ExceptionHandled = true;
            }
            else if ((filterContext.Exception as UtilsException) != null)
            {
                var utilsException = filterContext.Exception as UtilsException;
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    #region utilsException
                    var result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                    if (utilsException.ResultData == null)
                    {
                        result.Data = new
                        {
                            status = utilsException.ExceptionCode,
                            msg = utilsException.ExceptionMessage
                        };
                    }
                    else
                    {
                        result.Data = utilsException.ResultData;
                    }

                    filterContext.Result = result;
                    #endregion
                }
                else
                {
                    #region utilsException
                    ContentResult content = new ContentResult();

                    if (utilsException.ResultData == null)
                    {
                        content.Content = string.Concat("[",
                            utilsException.ExceptionCode,
                            "]",
                            utilsException.Message);
                    }
                    else
                    {
                        content.Content = utilsException.ResultData.ToJson();
                    }
                    #endregion
                    filterContext.Result = content;
                }

                //TODO:是否记录日志 后续开发

                filterContext.ExceptionHandled = true;
            }
        }

    }
}