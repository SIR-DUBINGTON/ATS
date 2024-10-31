using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    public enum DepartmentName  
    {
        Finance,
        HumanResources,
        Operations,
        Sales,
        InformationTechnology
    }

    public class Department
    {
        public int Id { get; set; }
        public DepartmentName Name { get; set; }  
        public List<User> Users { get; set; }

        public Department(int id, DepartmentName name, List<User> users) 
        {
            Id = id;
            Name = name;
            Users = users;
        }
    }
}
