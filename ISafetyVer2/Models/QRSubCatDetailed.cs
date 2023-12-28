using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISafetyVer2.Models
{
    public class QRSubCatDetailed
    {
        public SubCategory QRSubCat { get; set; }
        public List<QuickReport> QR { get; set; }
    }
}
