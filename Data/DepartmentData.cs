using System;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Data
{
    public sealed class DepartmentData : BaseEntityData
    {
        [StringLength(50)] public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public int? AdministratorId { get; set; }
    }
}