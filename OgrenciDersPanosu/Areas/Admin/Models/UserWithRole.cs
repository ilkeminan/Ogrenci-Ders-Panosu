using OgrenciDersPanosu.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Areas.Admin.Models
{
    public class UserWithRole
    {
        public ApplicationUser user { get; set; }
        public IList<string> Roles { get; set; }
    }

}