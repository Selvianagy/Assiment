using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assiment.core.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public class ApplicationUser: IdentityUser
    {
        public Gender gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
    }
}
