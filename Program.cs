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
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

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

  [HelpOption]
  [Command(ThrowOnUnexpectedArgument = false)]
  class Program
  {

    [Option(Description = "Output option to console", LongName = "console", ShortName = "c")]
    public bool Inline { get; set; } = true;

    [Option(Description = "Number of results 1 - 25", LongName = "number", ShortName = "n")]
    [Range(1, 25)]
    public int Results { get; set; } = 5;

    private string _terms = string.Empty;
    [Option(Description = "Search terms", ShortName = "t")]
    public string Terms
    {
      get
      {
        if (String.IsNullOrEmpty(_terms))
        {
          return String.Join("+", RemainingArguments).Trim();
        }
        else
        {
          return _terms;
        }
      }
      set
      {
        _terms = value;
      }
    }

    public string[] RemainingArguments { get; }

    public static int Main(string[] args)
        => CommandLineApplication.Execute<Program>(args);

    private void OnExecute(CommandLineApplication app)
    {
      if (RemainingArguments.Length == 0 && String.IsNullOrEmpty(Terms))
      {
        app.ShowHelp();
      }
      else
      {
        string locale = CultureInfo.CurrentCulture.Name;
        if (Inline)
        {
          string api = $"https://docs.microsoft.com/api/search?search={Terms}&locale={locale}&$top={Results}";
          Console.WriteLine(api);
          //return;

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
          var cmd = "open";
          string url = $"https://docs.microsoft.com/search/index?search={Terms}";

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

          var psi = new ProcessStartInfo { FileName = cmd, Arguments = url };
          var process = new Process { StartInfo = psi };
          process.Start();
        }
      }
    }

/*
    public static int Main_(string[] args)
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

      return 0;
    }
*/

    public async static Task<Docs> GetResults(string url)
    {
      var http = new HttpClient();
      var response = await http.GetStringAsync(url);

      var data = JsonConvert.DeserializeObject<Docs>(response);

      return data;
    }

  }
}

