using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PMSAPI.Models
{
    public class Doctor
    {
        [Key]
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Password { get; set; }
        public byte? DepartmentId { get; set; }
        public string Specializations { get; set; }
        public string Qualification { get; set; }
        public decimal? ConsultationFees { get; set; }

        public virtual Departments Department { get; set; }
    }
}
