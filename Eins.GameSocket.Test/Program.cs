using Eins.TransportEntities.Eins;
using Eins.TransportEntities.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eins.GameSocket.Test
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var testObject = new Test()
            {
                TestInt = 4,
                TestString = "Hallo"
            };
            var type = testObject.GetType();
            var propNames = type.GetProperties();
            await Task.Delay(1);
        }


        public static void Test(string test, Func<string,string> predicate)
        {
            Console.WriteLine(predicate.Invoke(test));
        }
    }

    public class Test
    {
        public int TestInt { get; set; }
        public string TestString { get; set; }
    }
}
