# docs

Global tool for search [https://docs.microsoft.com/?WT.mc_id=dotnetdoc-github-shboyer](https://docs.microsoft.com/?WT.mc_id=dotnetdoc-github-shboyer) using the command line.

## Installation

```bash
dotnet install tool -g dotnet-doc
```

## Usage

```bash
docs
Usage: docs [arguments] [options]

Arguments:
  Terms

Options:
  -?|-h|--help           Show help information
  -l|--lucky             Open first result
  -c|--console           Output option to console
  -n|--number <RESULTS>  Number of results 1 - 25
```

Example search for any **MVC** content

```bash
# opens default browser with search results from https://docs.microsoft.com/?WT.mc_id=dotnetdoc-github-shboyer
docs mvc
```

Return 5 results within console

```bash
docs mvc --console --number 5

Items found: 6584

ASP.NET MVC 2
https://docs.microsoft.com/aspnet/mvc/videos/mvc-2/?WT.mc_id=dotnetdoc-github-shboyer

ASP.NET MVC 1
https://docs.microsoft.com/aspnet/mvc/videos/mvc-1/?WT.mc_id=dotnetdoc-github-shboyer

ASP.NET MVC 4
https://docs.microsoft.com/aspnet/mvc/videos/mvc-4/?WT.mc_id=dotnetdoc-github-shboyer

Microsoft.AspNetCore.Mvc.ApplicationParts Namespace
https://docs.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.applicationparts?WT.mc_id=dotnetdoc-github-shboyer

Adding a View to an MVC app
https://docs.microsoft.com/aspnet/mvc/overview/getting-started/introduction/adding-a-view?WT.mc_id=dotnetdoc-github-shboyer
```

**Feeling Lucky?**

```bash
# open the first result
docs ".NET Docker" -l
```