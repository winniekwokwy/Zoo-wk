using System.Xml.Serialization;

public class CreateZooKeeperRequest
{
    public required string Name { get; set; }

    public required List<int> Enclosures { get; set; }
}