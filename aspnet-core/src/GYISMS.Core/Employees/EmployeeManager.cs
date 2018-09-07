

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.Employees;


namespace GYISMS.Employees
{
    /// <summary>
    /// Employee领域层的业务管理
    ///</summary>
    public class EmployeeManager :GYISMSDomainServiceBase, IEmployeeManager
    {
    private readonly IRepository<Employee,string> _employeeRepository;

        /// <summary>
            /// Employee的构造方法
            ///</summary>
        public EmployeeManager(IRepository<Employee, string>
employeeRepository)
            {
            _employeeRepository =  employeeRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitEmployee()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
