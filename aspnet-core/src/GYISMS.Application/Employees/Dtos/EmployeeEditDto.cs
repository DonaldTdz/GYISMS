

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using GYISMS.Employees;

namespace GYISMS.Employees.Dtos
{
    public class EmployeeEditDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }


        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Mobile
        /// </summary>
        public string Mobile { get; set; }


        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// Active
        /// </summary>
        public bool? Active { get; set; }


        /// <summary>
        /// IsAdmin
        /// </summary>
        public bool? IsAdmin { get; set; }


        /// <summary>
        /// IsBoss
        /// </summary>
        public bool? IsBoss { get; set; }


        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }
        public string DepartmentName { get; set; }


        /// <summary>
        /// Position
        /// </summary>
        public string Position { get; set; }


        /// <summary>
        /// Avatar
        /// </summary>
        public string Avatar { get; set; }


        /// <summary>
        /// HiredDate
        /// </summary>
        public string HiredDate { get; set; }


        /// <summary>
        /// Roles
        /// </summary>
        public string Roles { get; set; }


        /// <summary>
        /// RoleId
        /// </summary>
        public long? RoleId { get; set; }


        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }




        /// <summary>
        /// 区县Code
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 区县名称
        /// </summary>
        public string Area { get; set; }

        //// custom codes

        //// custom codes end
    }
}