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
        public int userId { get; set; }



        public SoftwareAsset(int id, int userId, string osName, string osVersion, string manufacturer)
        {
            this.id = id;
            this.osName = osName;
            this.osVersion = osVersion;
            this.manufacturer = manufacturer;
            this.userId = userId;
        }
    }
}
