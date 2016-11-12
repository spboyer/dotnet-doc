
using System.Collections.Generic;

public class Result
{
    public string title { get; set; }
    public string url { get; set; }
    public string description { get; set; }
    public string lastUpdatedDate { get; set; }
    public string iconType { get; set; }
    public List<object> breadcrumbs { get; set; }
}

public class Docs
{
    public List<Result> results { get; set; }
    public int count { get; set; }
}
