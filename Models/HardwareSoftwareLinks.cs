using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    public class HardwareSoftwareLinks
    {
        public int id { get; set; }
        public int hardwareAssetId { get; set; }
        public int softwareAssetId { get; set; }

    public HardwareSoftwareLinks(int id, int hardwareAssetId, int softwareAssetId)
        {
            this.id = id;
            this.hardwareAssetId = hardwareAssetId;
            this.softwareAssetId = softwareAssetId;
        }
    }
}
