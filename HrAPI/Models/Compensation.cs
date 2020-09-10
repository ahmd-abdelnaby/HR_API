using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace HrAPI.Models
{
    public class Compensation
    {
        public enum NeedsTYPE
        {
            Outstanding,
            Regular
        };
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }
        public string Description { get; set; }
        [AllowNull]
        public string Comment { get; set; }

    }
}
