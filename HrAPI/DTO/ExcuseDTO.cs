using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrAPI.DTO
{
    public class ExcuseDTO
    {
        public int ID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int Hours { get; set; }

        public string Profession { get; set; }
        public bool Approved { get; set; }
     
        public string Comment { get; set; }
    }
}
