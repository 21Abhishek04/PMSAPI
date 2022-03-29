using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PMSAPI.Models
{
    public partial class BloodGroups
    {
        public BloodGroups()
        {
            Patient = new HashSet<Patient>();
        }

        public byte BloodGroupId { get; set; }
        public string BloodGroup { get; set; }

        public virtual ICollection<Patient> Patient { get; set; }
    }
}
