using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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
        public int LeaveTypeID { get; set; }
        [ForeignKey("LeaveTypeID")]
        public LeavesTypes LeaveType { get; set; }
        public DateTime Start { get; set; }
        public DateTime Date { get; set; }
        public int Days { get; set; }

        public int AlternativeEmpID { get; set; }
        [ForeignKey("AlternativeEmpID")]
        public virtual Employee AlternativeEmp { get; set; }
        [AllowNull]
        public string Comment { get; set; }
        [AllowNull]
        public string AlternativeAddress { get; set; }
        public string Status { get; set; }
        //public List<string> LeavesFiles { get; set; }
        public  List<LeaveFiles> LeavesFiles { get; set; }
    }
}
