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

           string[] sites = {
           "google", "msdn",  "facebook", "linkedin", "twitter",
           "bing", "youtube",  "baidu",    "amazon", "gmail"};

            var a = sites.Select(x => new Uri(string.Format(@"http://{0}.com", x)));

            a.GetUrlContentAsync(3);


            Console.ReadLine();
        }
    }





}
