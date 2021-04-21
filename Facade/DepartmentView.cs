﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contoso.Facade
{
    public sealed class DepartmentView : NamedView
    {

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")] public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")] public DateTime StartDate { get; set; }

        [Display(Name = "Administrator")]
        public int? AdministratorId { get; set; }

        [Display(Name = "Administrator")]
        public string AdministratorName { get; set; }
    }
}