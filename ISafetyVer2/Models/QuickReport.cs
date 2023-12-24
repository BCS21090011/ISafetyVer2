using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISafetyVer2.Models
{
    public class QuickReport
    {
        public string QRID { get; set; }
        public string UserID { get; set; }
        public string SubCatID { get; set; }
        public DateTime ReportDateTime { get; set; }
        public string QRDescription { get; set; }
        public double Latitude {  get; set; }
        public double Longitude { get; set; }
        public double Radius {  get; set; }
        public string MediaURL {  get; set; }
        public string MediaType { get; set; }
        public string Status { get; set; }
    }
}
