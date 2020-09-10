using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrAPI.Models
{
    public class LeaveRequest
    {
        public enum LeaveTYPE
        {
            EarlyDismiss,
            LateAttend
        };
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }
        public string LeaveType { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
