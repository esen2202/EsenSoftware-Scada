using EldorST18.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EldorST18.core
{
    public class DB : IDisposable
    {

        public List<User> GetUser()
        {
            using (eldorST18Entities db = new eldorST18Entities())
            {
                return db.Users.ToList();
            }
        }

        public bool SignIn(string uName, string uPass)
        {
            using (eldorST18Entities db = new eldorST18Entities())
            {
                var user =
                 db.Users.Where(record => record.UserName == uName && record.UserPassword == uPass);
                return user.Any();
            }
        }

        public bool SignIn(string carId)
        {

            using (eldorST18Entities db = new eldorST18Entities())
            {
                var user =
                 db.Users.Where(record => record.CardID == carId);
                return user.Any();
            }

        }

        public sbyte AddUser(model.User user)
        {
            sbyte result;
            using (eldorST18Entities db = new eldorST18Entities())
            {
                if (!db.Users.Any(record => record.UserName == user.UserName || record.CardID == user.CardID))
                {
                    try
                    {
                        user.UserPassword = CryptorEngine.Encrypt(user.UserPassword, true);
                        user.User_ID = 0;
                        user.DateTime = DateTime.Now;
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result = 0;
                    }
                    result = 1;
                }
                else
                {
                    result = -1;
                }
            }
            return result;
        }

        void IDisposable.Dispose()
        {

        }
    }
}
