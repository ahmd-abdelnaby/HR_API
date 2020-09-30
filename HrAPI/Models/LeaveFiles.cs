using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace HrAPI.Models
{
    public class LeaveFiles
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public int LeaveRequestID { get; set; }
        [ForeignKey("LeaveRequestID")]
        public LeaveRequest LeaveRequest { get; set; }
    }
}
