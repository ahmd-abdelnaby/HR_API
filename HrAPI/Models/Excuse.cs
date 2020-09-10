﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace HrAPI.Models
{
    public class Excuse
    {
        public enum ExcuseTYPE
        {
            EarlyDismiss,
            LateAttend
        };
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }
        public DateTime Date { get; set; }
        public DateTime Arrival { get; set; }
        [AllowNull]
        public string  Comment { get; set; }
    }
}
