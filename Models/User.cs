using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public string department { get; set; }
        public string role { get; set; }
        public List<Asset> Assets { get; set; }

        public User(int id, string username, string password, string firstName, string lastName, string emailAddress, string department, string role, List<Asset> Assets)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.firstName = firstName;
            this.lastName = lastName;
            this.emailAddress = emailAddress;
            this.department = department;
            this.role = role;
            this.Assets = Assets;
        }
    }
}
