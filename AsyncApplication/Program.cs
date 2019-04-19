using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace AsyncApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Tasks.GetMD5Async(new Uri("https://www.yandex.ru"));

            Console.ReadLine();
        }
    }





}
