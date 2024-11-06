using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    /// <summary>
    /// The department name enumeration as the department are static.
    /// </summary>
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
    /// <summary>
    /// This class is used to get the description of the department name enumeration.
    /// </summary>
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
    /// <summary>
    /// This class is used to create a department object.
    /// </summary>
    public class Department
    {
        public int Id { get; set; }
        public DepartmentName Name { get; set; }
        public List<User> Users { get; set; }
        /// <summary>
        /// Constructor for the users lists dedicated to a department class.
        /// </summary>
        public Department()
        {
            Users = new List<User>();
        }
        /// <summary>
        /// This constructor is used to create a department object.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="users"></param>
        public Department(int id, DepartmentName name, List<User> users)
        {
            Id = id;
            Name = name;
            Users = new List<User>();
        }
        /// <summary>
        /// Method to get the department name string.
        /// </summary>
        /// <returns>Name.ToString</returns>
        public string GetDepartmentNameString()
        {
            return Name.ToString();
        }
    }
}

