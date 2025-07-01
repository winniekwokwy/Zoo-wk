using System.Xml.Serialization;

public class SearchTransferRequest : SearchRequest
{
    public SearchTransferRecord? search { get; set; }

}