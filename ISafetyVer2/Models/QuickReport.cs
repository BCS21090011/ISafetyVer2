﻿using System;
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
        public decimal Latitude {  get; set; }
        public decimal Longitude { get; set; }
        public string QRAddress { get; set; }
    }
}