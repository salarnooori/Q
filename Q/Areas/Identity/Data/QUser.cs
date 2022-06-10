using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Q.Areas.Identity.Data;

// Add profile data for application users by adding properties to the QUser class
public class QUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "int")]
    public int Experience { get; set; }

    [PersonalData]
    [Column(TypeName = "int")]
    public int ThemeId { get; set; }
}

