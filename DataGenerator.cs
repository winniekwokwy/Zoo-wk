using Bogus;
using Microsoft.EntityFrameworkCore;

public class DataGenerator
{

    public static void GenerateData(ZooDbContext db)
    {
        var faker = new Faker<Animal>()
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Sex, f => "Male")
            .RuleFor(u => u.SpeciesId, f => 1)
            .RuleFor(u => u.DateofBirth, f => DateOnly.FromDateTime(f.Date.Past()))
            .RuleFor(u => u.DateofAcquisition, f => DateOnly.FromDateTime(f.Date.Past()))
            .RuleFor(u => u.EnclosureId, f => 1);

        var res = faker.Generate(100);

        res.ForEach(animal =>
        {
            db.Animals.Add(animal);
            db.SaveChanges();
        });
    }
}