using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    public class SoftwareAsset
    {
        public int id { get; set; }
        public string osName { get; set; }
        public string osVersion { get; set; }
        public string manufacturer { get; set; }


        public SoftwareAsset(int id, string osName, string osVersion, string manufacturer)
        {
            this.id = id;
            this.osName = osName;
            this.osVersion = osVersion;
            this.manufacturer = manufacturer;

        }
    }
}
