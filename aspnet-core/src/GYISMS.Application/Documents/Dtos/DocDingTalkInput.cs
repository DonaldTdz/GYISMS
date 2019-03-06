using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using GYISMS.DocDingTalks;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Documents.Dtos
{
    [AutoMapTo(typeof(DocDingTalk))]
    public class DocDingTalkInput
    {
        /// <summary>
        /// 文档附件Id
        /// </summary>
        public Guid DocAttId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 钉盘空间Id
        /// </summary>
        public string SpaceId { get; set; }

        /// <summary>
        /// 钉盘文件Id
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 钉盘文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 钉盘文件扩展名
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 钉盘文件大小
        /// </summary>
        public long? FileSize { get; set; }

    }
}
