public class Animal
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public required string Sex { get; set; }

    public required int SpeciesId { get; set; }
    public Species? species { get; set; }

    private DateOnly _dateOfBirth;
    public required DateOnly DateofBirth
    {
        get => _dateOfBirth;
        set
        {
            _dateOfBirth = value;
            Age = CalculateAge(_dateOfBirth); // Automatically calculate age when DateofBirth is set
        }
    }
    public required DateOnly DateofAcquisition { get; set; }

    public required int EnclosureId { get; set; }
    public Enclosure? enclosure { get; set; }
    public int Age { get; private set; } // Make Age a read-only property

    private int CalculateAge(DateOnly birthday)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        int age = today.Year - birthday.Year;
        if (today < birthday.AddYears(age))
        {
            age--;
        }
        return age;
    }

}