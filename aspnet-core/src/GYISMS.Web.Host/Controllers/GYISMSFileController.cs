using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Authorization;
using HC.WeChat.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;

namespace GYISMS.Web.Host.Controllers
{
    public class GYISMSFileController : AbpController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GYISMSFileController(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 会议室图片上传
        /// </summary>
        /// <param name="image"></param>
        /// <param name="fileName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        [HttpPost]
        public async Task<IActionResult> MeetingRoomPost(IFormFile[] image, string fileName, Guid name)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var imageName = "";
            foreach (var formFile in image)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    name = name == Guid.Empty ? Guid.NewGuid() : name;
                    string newName = name + fileExt; //新的文件名
                    var fileDire = webRootPath + string.Format("/meetingRooms/{0}/", fileName);
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }

                    var filePath = fileDire + newName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    imageName = filePath.Substring(webRootPath.Length);
                }
            }

            return Ok(new { imageName });
        }

        [HttpPost]
        [AbpAllowAnonymous]
        [Audited]
        public async Task<IActionResult> FilesPostsBase64Async([FromBody]ImgBase64 input)
        {
            if (!string.IsNullOrWhiteSpace(input.imageBase64))
            {
                var reg = new Regex("data:image/(.*);base64,");
                input.imageBase64 = reg.Replace(input.imageBase64, "");
                byte[] imageByte = Convert.FromBase64String(input.imageBase64);
                var memorystream = new MemoryStream(imageByte);

                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                string fileExt = Path.GetExtension(input.fileName); //文件扩展名，不含“.”
                string newFileName = Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                var fileDire = webRootPath + "/visit/";
                if (!Directory.Exists(fileDire))
                {
                    Directory.CreateDirectory(fileDire);
                }

                var filePath = fileDire + newFileName;
                ////2018-7-6 压缩后保存
                //using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load(imageByte))
                //{
                //    //如果高度大于200 就需要压缩
                //    if (image.Height > 200)
                //    {
                //        var width = (int)((200 / image.Height) * image.Width);
                //        image.Mutate(x => x.Resize(width, 200));
                //    }
                //    image.Save(filePath);
                //}
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await memorystream.CopyToAsync(stream);
                }
                var saveUrl = filePath.Substring(webRootPath.Length);
                return Ok(saveUrl);
            }
            return Ok();
        }

        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        [HttpPost]
        [AbpAllowAnonymous]
        [Audited]
        public Task<IActionResult> FilesPostsAsync(IFormFile[] file)
        {
            var date = Request;
            var files = Request.Form.Files;
            //long size = files.Sum(f => f.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var filePath = string.Empty;
            var returnUrl = string.Empty;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    var uid = Guid.NewGuid().ToString();
                    string newFileName = uid + fileExt; //随机生成新的文件名
                    var fileDire = webRootPath + "/visit/";
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }

                    filePath = fileDire + newFileName;

                    //2压缩后保存 跨度为360px
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load(formFile.OpenReadStream()))
                    {
                        //如果宽度度大于360 就需要压缩
                        if (image.Width > 360)
                        {
                            var height = (int)((360 / image.Width) * image.Height);
                            image.Mutate(x => x.Resize(360, height));
                        }
                        image.Save(filePath);
                    }

                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await formFile.CopyToAsync(stream);
                    //}

                    returnUrl = "/visit/" + newFileName;
                }
            }

            return Task.FromResult((IActionResult)Ok(returnUrl));
        }

    }

    public class ImgBase64
    {
        public string fileName { get; set; }

        public string imageBase64 { get; set; }
    }
}