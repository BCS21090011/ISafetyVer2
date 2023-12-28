using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISafetyVer2.Models
{
    public class QRCatDetailed
    {
        public Category QRCat { get; set; }
        public List<QRSubCatDetailed> QRSubCat { get; set; }
    }
}
