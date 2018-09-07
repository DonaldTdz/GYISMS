

using AutoMapper;
using GYISMS.Employees;
using GYISMS.Employees;

namespace GYISMS.Employees.Dtos.CustomMapper
{

	/// <summary>
    /// 配置Employee的AutoMapper
    ///</summary>
	internal static class CustomerEmployeeMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Employee, EmployeeListDto>
    ();
    configuration.CreateMap <EmployeeEditDto, Employee>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
