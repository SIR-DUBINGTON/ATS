using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    public class Department
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<User> Users { get; set; }

        public Department(int id, string name, List<User> Users)
        {
            this.id = id;
            this.name = name;
            this.Users = Users;
        }
    }
}
