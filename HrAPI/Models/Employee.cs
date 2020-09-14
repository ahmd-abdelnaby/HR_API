using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrAPI.Models
{
    public class Employee 
    {
        public enum Gender
        {
            Male,
            Female
        };
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProfessionID { get; set; }
        [ForeignKey("ProfessionID")]
        public virtual Profession Profession { get; set; }
        public string gender { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string GraduatioYear { get; set; }
        public string Phone { get; set; }
        public string RelevantPhone { get; set; }
        public string Email { get; set; }
        public string photo { get; set; }
        public  DateTime HiringDateHiringDate { get; set; }
    }
}
