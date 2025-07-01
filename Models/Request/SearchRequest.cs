using System.Xml.Serialization;

public class SearchRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SortBy { get; set; } = "species";

}