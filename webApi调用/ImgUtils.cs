using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace webApi调用
{
    class ImgUtils
    {
        private static char[] base64CodeArray = new char[]
       {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4',  '5', '6', '7', '8', '9', '+', '/', '='
       };
        public static bool IsBase64(string base64Str)
        {
            //string strRegex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$";

            if (string.IsNullOrEmpty(base64Str))
                return false;
            else
            {
                if (base64Str.Contains(","))
                    base64Str = base64Str.Split(',')[1];
                if (base64Str.Length % 4 != 0)
                    return false;
                if (base64Str.Any(c => !base64CodeArray.Contains(c)))
                    return false;
            }
            try
            {
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static string ConvertImageToBase64(Image bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length]; ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length); ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return "转换错误";
            }
        }
        public static Image ConvertBase64ToImage(string base64String)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    return Image.FromStream(ms, true);
                    }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Image resizeImg(Image sourceImage, int targetWidth, int targetHeight)
        {
            int width;//图片最终的宽
            int height;//图片最终的高
            try
            {
                //获取图片宽度
                int sourceWidth = sourceImage.Width;
                //获取图片高度
                int sourceHeight = sourceImage.Height;

                System.Drawing.Imaging.ImageFormat format = sourceImage.RawFormat;
                Bitmap targetPicture = new Bitmap(targetWidth, targetHeight);
                Graphics g = Graphics.FromImage(targetPicture);
                g.Clear(Color.White);
                g.DrawImage(sourceImage, 0,0, targetWidth, targetHeight);
                sourceImage.Dispose();
                return targetPicture;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

    }
}
