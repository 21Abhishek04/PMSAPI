using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PMSAPI.Models
{
    public partial class Departments
    {
        public Departments()
        {
            Appointment = new HashSet<Appointment>();
            Doctor = new HashSet<Doctor>();
        }

        public byte DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<Doctor> Doctor { get; set; }
    }
}
