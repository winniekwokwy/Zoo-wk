public class Animal
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public required string Sex { get; set; }

    public required int SpeciesId { get; set; }

    public required DateOnly DateofBirth { get; set; }

    public required DateOnly DateofAcquisition { get; set; }

    public required int EnclosureId { get; set; }
}