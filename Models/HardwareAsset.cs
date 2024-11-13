using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ATS.Models
{
    /// <summary>
    /// This class represents an asset in the system.
    /// </summary>
    public class HardwareAsset
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

        ///public override string ToString()
        ///{
        /// return $"{name} - {model} - {manufacturer} - {type} - {ip} - {purchaseDate} - {textNotes}";
        ///}

        /// <summary>
        /// Constructor for the Asset class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <param name="manufacturer"></param>
        /// <param name="type"></param>
        /// <param name="ip"></param>
        /// <param name="purchaseDate"></param>
        /// <param name="textNotes"></param>
        public HardwareAsset(int id, int userId, string name, string model, string manufacturer, string type, string ip, DateTime purchaseDate, string textNotes)
        {
            this.id = id;
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
