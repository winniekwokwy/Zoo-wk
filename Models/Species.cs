
public class Species
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public required int ClassificationId { get; set; }

    public Classification? classification { get; set; }
}