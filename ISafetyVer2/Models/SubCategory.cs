﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISafetyVer2.Models
{
    internal class SubCategory
    {
        public string SubCatID { get; set; }
        public string CategoryID { get; set; }
        public string SubCatName { get; set; }
        public int AreaRadius { get; set; }
        public int DangerLvl { get; set; }
    }
}