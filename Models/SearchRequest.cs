using System.Xml.Serialization;

public class SearchRequest
{
    public Search? search { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SortBy { get; set; } = "species";

}