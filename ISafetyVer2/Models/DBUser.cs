using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISafetyVer2.Models
{
    public class DBUser
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public string Role { get; set; }
    }
}
