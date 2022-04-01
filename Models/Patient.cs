using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PMSAPI.Models
{
    public partial class Patient
    {
        [Key]
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string Password { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Gender { get; set; }
        public decimal? Weight { get; set; }
        public byte? BloodGroupId { get; set; }
        public long? PhoneNumber { get; set; }

        public virtual BloodGroups BloodGroup { get; set; }
    }
}
