using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scada.model;

namespace Scada.core
{
    public class DB : IDisposable
    {
        //DB Operations 
        public bool SignIn(string text, string password, out User userRecord)
        {
            //throw new NotImplementedException();
            userRecord = new User();
            userRecord.UserName = text;
            userRecord.UserPassword = password;
            userRecord.Authorization = 3;
            return true;
        }

        public IEnumerable GetEditableUsers(short authControlLevel, long user_ID)
        {
            // throw new NotImplementedException();

            return new List<User>();

        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public object UpdateUser(User user)
        {
            return 1;
        }

        public object DeleteUser(long selectedValue)
        {
            return 1;
        }

        public User GetUser(long selectedValue, long user_ID)
        {
            return new User();
        }

        public object AddUser(User user)
        {
            return 1;
        }
    }
}
