using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrAPI.ViewModels
{
    public class LeaveRequestVM
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }

        public int LeaveTypeID { get; set; }
    
        public DateTime Start { get; set; }
        public DateTime Date { get; set; }
        public int Days { get; set; }

        public int AlternativeEmpID { get; set; }
    
        public string Comment { get; set; }
     
        public string AlternativeAddress { get; set; }
        public string Status { get; set; }
        //public List<string> LeavesFiles { get; set; }
        public string LeavesFiles { get; set; }
    }
}
