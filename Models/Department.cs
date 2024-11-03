using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    public enum DepartmentName
    {
        [Description("Finance")]
        Finance,

        [Description("Human Resources")]
        HumanResources,

        [Description("Operations")]
        Operations,

        [Description("Sales")]
        Sales,

        [Description("Information Technology")]
        InformationTechnology
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                DescriptionAttribute attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                return attr != null ? attr.Description : value.ToString();
            }
            return value.ToString();
        }
    }
    public class Department
    {
        public int Id { get; set; }
        public DepartmentName Name { get; set; }
        public List<User> Users { get; set; }

        public Department()
        {
            Users = new List<User>();
        }

        public Department(int id, DepartmentName name, List<User> users)
        {
            Id = id;
            Name = name;
            Users = new List<User>();
        }

        public string GetDepartmentNameString()
        {
            return Name.ToString();
        }
    }
}

