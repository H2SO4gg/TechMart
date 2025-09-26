using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMart.Common
{
    public static class LoggedInUser
    {
        public static int UserID { get; set; }
        public static string Email { get; set; }
        public static string Role { get; set; }
    }
}
