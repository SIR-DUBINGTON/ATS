using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Helpers
{
    /// <summary>
    /// This class is used to hash and verify the password for the users
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Hashes the password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Password</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        /// <summary>
        ///  Verifies the password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns>Password and HashedPassword</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
