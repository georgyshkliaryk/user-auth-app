using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UserAuthApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Nickname { get; set; }

        [PersonalData]
        [Column(TypeName = "datetime")]
        public virtual DateTime? LastLoginTime { get; set; }

        [PersonalData]
        [Column(TypeName = "datetime")]
        public virtual DateTime? RegistrationDate { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Status { get; set; }
    }
}
