using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models
{
    public class SessionManager
    {
        private static SessionManager _instance;

        public int UserId { get; private set; }
        public string Username { get; private set; }

        private SessionManager(int userId, string username)
        {
            UserId = userId;
            Username = username;
        }

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
