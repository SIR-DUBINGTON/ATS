using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    public class Tracker
    {
        public int id { get; set; }
        public string department { get; set; }
        public string user { get; set; }

        public Tracker(int id, string department, string user)
        {
            this.id = id;
            this.department = department;
            this.user = user;
        }

    }
}
