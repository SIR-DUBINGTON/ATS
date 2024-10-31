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
        public string passwordHash { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public string department { get; set; }
        public DepartmentName Department { get; set; }
        public string role { get; set; }
        public List<Asset> Assets { get; set; }

        public User(int id, string username, string passwordHash, string firstName, string lastName, string emailAddress, DepartmentName department, string role, List<Asset> Assets)
        {
            this.id = id;
            this.username = username;
            this.passwordHash = passwordHash;
            this.firstName = firstName;
            this.lastName = lastName;
            this.emailAddress = emailAddress;
            this.Department = department;
            this.role = role;
            this.Assets = Assets;
        }
    }
}
