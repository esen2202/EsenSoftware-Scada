using EldorST18.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorST18.test
{
    class Program
    {
        static void Main(string[] args)
        {
            core.DB db = new core.DB();
            List<User> a = db.GetUser();
            Console.WriteLine(a[0].Name+ " " + a[0].Surname);
            Console.ReadLine();
        }
    }
}
