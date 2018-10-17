
using AutoMapper;
using GYISMS.DocAttachments;
using GYISMS.DocAttachments.Dtos;

namespace GYISMS.DocAttachments.Mapper
{

	/// <summary>
    /// 配置DocAttachment的AutoMapper
    /// </summary>
	internal static class DocAttachmentMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <DocAttachment,DocAttachmentListDto>();
            configuration.CreateMap <DocAttachmentListDto,DocAttachment>();

            configuration.CreateMap <DocAttachmentEditDto,DocAttachment>();
            configuration.CreateMap <DocAttachment,DocAttachmentEditDto>();

        }
	}
}
