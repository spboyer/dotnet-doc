using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace ConsoleApplication
{
    //launch - https://docs.microsoft.com/search/index?search=docker
    //api - https://docs.microsoft.com/api/search?search=mvc&locale=en-us&$top=10

    // {
    // title: "Building your first ASP.NET Core MVC app with Visual Studio",
    // url: "https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/",
    // description: "Getting started with ASP.NET Core MVC and Visual Studio",
    // lastUpdatedDate: "2016-10-14T00:00:00+00:00",
    // iconType: "Article",
    // breadcrumbs: [ ]
    // },
    // {
    // title: "Microsoft.AspNetCore.Mvc.Infrastructure namespace",
    // url: "https://docs.microsoft.com/en-us/aspnet/core/api/microsoft.aspnetcore.mvc.infrastructure",
    // description: null,
    // lastUpdatedDate: "2016-09-15T00:00:00+00:00",
    // iconType: "Reference",
    // breadcrumbs: [ ]
    // },

    public class Program
    {
        public static void Main(string[] args)
        {

            var csl = args.Any(a => a == "--console");
        


            string locale = CultureInfo.CurrentCulture.Name;

            string cmd = "open";
            string terms = Parse(args);
            string url = $"https://docs.microsoft.com/search/index?search={terms}";
            string api = $"https://docs.microsoft.com/api/search?search={terms}&local={locale}&$top=50";



            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                cmd = "xdg-open";
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {

                cmd = "open";
            }


            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {

                cmd = "start";
            }



            if (csl)
            {
                var items = GetResults(api);

                Console.WriteLine("Items found: " + items.Result.count.ToString());
                foreach (Result r in items.Result.results)
                {
                    Console.WriteLine(r.iconType);
                    Console.WriteLine(r.title);
                    Console.WriteLine(r.url);
                    Console.WriteLine(" ");
                }
            }
            else
            {
                var psi = new ProcessStartInfo
                {
                    FileName = cmd,
                    Arguments = url
                };

                var process = new Process
                {
                    StartInfo = psi,

                };
                process.Start();
            }
        }
        private static string Parse(string[] args)
        {
            StringBuilder searchArg = new StringBuilder();

            for (int i = 0; i < args.Length; i++)
            {
                searchArg.AppendFormat("{0} ", args[i].Replace(" ", "+").Replace("--console", ""));
            }

            return searchArg.ToString().TrimEnd();
        }

        public async static Task<Docs> GetResults(string url)
        {
            url = "docs.json";
            //var http = new HttpClient();
            //var response = await http.GetStringAsync(url);


            //Console.Write(response);

            var response = File.ReadAllText("docs.json");
            var data = JsonConvert.DeserializeObject<Docs>(response);

            return data;
        }

    }
}

