using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PMSAPI.Models
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }
        public string PatientId { get; set; }
        public byte? DepartmentId { get; set; }
        public string DoctorId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Status { get; set; }
        public decimal? ConsultationFees { get; set; }

        public virtual Departments Department { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
