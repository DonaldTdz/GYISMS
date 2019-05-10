using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GYISMS.Helpers
{
    public class ImageHelper
    {
        public static string GetWeekDay(DayOfWeek dayWeek)
        {
            switch (dayWeek)
            {
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Saturday:
                    return "星期六";
                case DayOfWeek.Sunday:
                    return "星期日";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                default:
                    return string.Empty;
            }
        }

        public static string GenerateWatermarkImg(string[] imgPaths, string location, string userName, string growerName, string host)
        {
            var imgpaths = string.Empty;
            int i = 1;
            foreach (var imgPath in imgPaths)
            {
                //拜访时间
                DateTime stime = DateTime.Now;
                //var host = _hostingEnvironment.WebRootPath;
                var imgFullPath = host + imgPath;
                using (FileStream stream = File.OpenRead(imgFullPath))
                using (Image<Rgba32> vimage = Image.Load(stream))
                {
                    //画文字
                    var fontCollection = new FontCollection();
                    var fontPath = "C:/Windows/Fonts/simsunb.ttf";
                    //var fontPath = "C:/Windows/Fonts/STXINWEI.TTF";
                    //var fontPath = "C:/Windows/Fonts/simfang.ttf";
                    var fontTitle = new Font(fontCollection.Install(fontPath), 20, FontStyle.Bold);
                    var font = new Font(fontCollection.Install(fontPath), 14, FontStyle.Bold);
                    //var fontTitle = SystemFonts.CreateFont("Microsoft YaHei UI", 20, FontStyle.Bold);
                    //var font = SystemFonts.CreateFont("Microsoft YaHei UI", 12, FontStyle.Bold);
                    vimage.Mutate(x => x.DrawText(stime.ToString("HH:mm"), fontTitle, Rgba32.WhiteSmoke, new PointF(10, 5)));
                    vimage.Mutate(x => x.DrawText(string.Format("{0} {1}", stime.ToString("yyyy.MM.dd"), GetWeekDay(stime.DayOfWeek)), font, Rgba32.WhiteSmoke, new PointF(10, 30)));
                    vimage.Mutate(x => x.DrawText(string.Format("拜访烟农: {0}", growerName), font, Rgba32.WhiteSmoke, new PointF(10, 48)));
                    TextGraphicsOptions options = new TextGraphicsOptions(true)
                    {
                        Antialias = true,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    var height = vimage.Height;
                    vimage.Mutate(x => x.DrawText(options, "用户: " + userName, font, Rgba32.WhiteSmoke, new PointF(350, height - 46)));
                    vimage.Mutate(x => x.DrawText(options, "位置: " + location, font, Rgba32.WhiteSmoke, new PointF(350, height - 28)));
                    var newImagePath = imgPath.Replace("visit", "visit/watermark");
                    var newFolder = host + "/visit/watermark";
                    if (!Directory.Exists(newFolder))
                    {
                        Directory.CreateDirectory(newFolder);
                    }
                    vimage.Save(host + newImagePath);
                    imgpaths += newImagePath;
                    if (i != imgPaths.Length)
                    {
                        imgpaths += ",";
                    }
                    i++;
                }
            }
            return imgpaths;
        }

        public static string GenerateWatermarkImg(string imgPaths, string location, string userName, string growerName, string host)
        {
            var imgs = imgPaths.Split(',');
            return GenerateWatermarkImg(imgs, location, userName, growerName, host);
        }
    }
}
