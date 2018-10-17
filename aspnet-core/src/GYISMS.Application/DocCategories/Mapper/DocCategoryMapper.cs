
using AutoMapper;
using GYISMS.DocCategories;
using GYISMS.DocCategories.Dtos;

namespace GYISMS.DocCategories.Mapper
{

	/// <summary>
    /// 配置DocCategory的AutoMapper
    /// </summary>
	internal static class DocCategoryMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <DocCategory,DocCategoryListDto>();
            configuration.CreateMap <DocCategoryListDto,DocCategory>();

            configuration.CreateMap <DocCategoryEditDto,DocCategory>();
            configuration.CreateMap <DocCategory,DocCategoryEditDto>();

        }
	}
}
