using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISafetyVer2.Models
{
    public class QRDetailed
    {
        public QuickReport QR { get; set; }
        public SubCategory QRSubCat { get; set; }
        public Category QRCat { get; set; }
    }
}
