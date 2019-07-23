using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Framework.Mayiboy.Utility;

namespace Mayiboy.IM.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class HeadimgHelper
    {
        /// <summary>
        /// 生成头像
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static byte[] Generate(string value)
        {
            using (Bitmap imgOutput = new Bitmap(200, 200, PixelFormat.Format48bppRgb))
            {
                using (Graphics g = Graphics.FromImage(imgOutput))
                {
                    g.Clear(Color.FromArgb(RandomHelper.NextInt(10, 200), RandomHelper.NextInt(10, 200), RandomHelper.NextInt(10, 200)));
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center; //居中
                    format.LineAlignment = StringAlignment.Center;

                    g.DrawString(value, new Font("楷体", 80, FontStyle.Bold), Brushes.White, new Rectangle(0, 0, 200, 200), format);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        imgOutput.Save(stream, ImageFormat.Jpeg);
                        return stream.ToArray();
                    }
                }
            }
        }
    }
}