using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    public class Asset
    {
        public int id { get; set; }
        public string name { get; set; }
        public string model { get; set; }
        public string manufacturer { get; set; }
        public string type { get; set; }
        public string ip { get; set; }
        public DateTime purchaseDate { get; set; }
        public string textNotes { get; set; }

        public int userId { get; set; } 
        public Asset(int userId, string name, string model, string manufacturer, string type, string ip, DateTime purchaseDate, string textNotes)
        {
            this.userId = userId;
            this.name = name;
            this.model = model;
            this.manufacturer = manufacturer;
            this.type = type;
            this.ip = ip;
            this.purchaseDate = purchaseDate;
            this.textNotes = textNotes;
        }

    }
}
