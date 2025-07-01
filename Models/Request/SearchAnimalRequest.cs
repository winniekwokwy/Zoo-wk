using System.Xml.Serialization;

public class SearchAnimalRequest : SearchRequest
{
    public AnimalSearch? search { get; set; }

}