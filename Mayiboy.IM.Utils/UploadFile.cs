using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Framework.Mayiboy.Utility;

namespace Mayiboy.IM.Utils
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadFile
    {

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="serverPath">服务器路径</param>
        /// <param name="msg">信息</param>
        /// <returns></returns>
        public static void SaveFile(byte[] data, string serverPath, out string msg)
        {
            msg = string.Empty;
            try
            {
                var directory = Path.GetDirectoryName(serverPath);

                #region 完整文件夹路径
                if (!Directory.Exists(directory))
                {
                    if (directory != null)
                    {
                        Directory.CreateDirectory(directory);
                    }
                    else
                    {
                        msg = "文件路径为空";
                    }
                }
                #endregion

                using (FileStream fs = new FileStream(serverPath, FileMode.Create))
                {
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="module">模块</param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string SaveFile(HttpPostedFileBase file, string module, string filename)
        {
            try
            {
                byte[] data = new byte[file.ContentLength];

                file.InputStream.Read(data, 0, data.Length);

                var fileinfo = new FileInfo(file.FileName);

                var fullfilename = filename + fileinfo.Extension;

                string serverFileName = AppConfig.FileRootPath + "\\" + module.Trim() + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + fullfilename;
                string httpfileurl = AppConfig.HttpFileUrl + "/" + module.Trim() + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + fullfilename;

                var msg = "";
                SaveFile(data, serverFileName, out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    throw new System.Exception(msg);
                }

                return httpfileurl;
            }
            catch (System.Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("上传文件保存文件出错：{0}", new { err = ex.ToString() }.ToJson());
                return "";
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="module">模块</param>
        /// <returns></returns>
        public static string SaveFile(HttpPostedFileBase file, string module)
        {
            return SaveFile(file, module, Guid.NewGuid().ToString("N"));
        }


    }
}