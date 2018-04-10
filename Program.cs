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
  [HelpOption]
  [Command(ThrowOnUnexpectedArgument = false)]
  class Program
  {

    [Option(Description = "Open first result", LongName = "lucky", ShortName = "l")]
    public bool Lucky { get; set; } = true;

    [Option(Description = "Output option to console", LongName = "console", ShortName = "c")]
    public bool Inline { get; set; } = true;

    [Option(Description = "Number of results 1 - 25", LongName = "number", ShortName = "n")]
    [Range(1, 25)]
    public int Results { get; set; } = 5;

    [Argument(0)]
    public string Terms { get; set; }

    public string[] RemainingArguments { get; }

    public static int Main(string[] args)
        => CommandLineApplication.Execute<Program>(args);

    private void OnExecute(CommandLineApplication app)
    {
      if (String.IsNullOrEmpty(Terms))
      {
        app.ShowHelp();
      }
      else
      {
        string locale = CultureInfo.CurrentCulture.Name;
        if (Inline | Lucky)
        {
          string api = $"https://docs.microsoft.com/api/search?search={Terms}&locale={locale}&$top={Results}";
          Console.WriteLine(api);

          var items = GetResults(api);

          if (Lucky)
          {
            OpenResult(items.Result.results[0].url);
          }

          if (Inline)
          {
            Console.WriteLine("Items found: " + items.Result.count.ToString());
            foreach (Result r in items.Result.results)
            {
              Console.WriteLine(r.iconType);
              Console.WriteLine(r.title);
              Console.WriteLine(r.url);
              Console.WriteLine(" ");
            }
          }
        }
        else
        {
          string url = $"https://docs.microsoft.com/search/index?search={Terms}";
          OpenResult(url);
        }
      }
    }

    private void OpenResult(string url)
    {
      var cmd = "open";

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

    public async static Task<Docs> GetResults(string url)
    {
      var http = new HttpClient();
      var response = await http.GetStringAsync(url);

      var data = JsonConvert.DeserializeObject<Docs>(response);

      return data;
    }

  }
}


