using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISafetyVer2.Models
{
    public class AdminPost
    {
        public string PostID { get; set; }
        public string PostTitle { get; set; }

        public string ReportDateTime { get; set; }
        public string PostDescription { get; set; }

        public string Location { get; set; }

        public string MediaURL { get; set; }
        
    }
}
