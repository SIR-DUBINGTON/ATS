using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    /// <summary>
    /// This class is used to manage the session of the user.
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// Field to store the instance of the SessionManager class.
        /// </summary>
        private static SessionManager _instance;
        /// <summary>
        /// Securely setup constructor to initialize the SessionManager class.
        /// </summary>
        public int UserId { get; private set; }
        public string Username { get; private set; }
        private SessionManager(int userId, string username)
        {
            UserId = userId;
            Username = username;
        }
        /// <summary>
        /// Method to initialize the SessionManager class.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        public static void Initialize(int userId, string username)
        {
            if (_instance == null)
            {
                _instance = new SessionManager(userId, username);
            }
            else
            {
                _instance.UserId = userId;
                _instance.Username = username;
            }
        }

        public static SessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("SessionManager has not been initialized.");
                }
                return _instance;
            }
        }
    }
}
