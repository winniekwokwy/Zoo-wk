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

        var EnclosureId = 1;
        var maxCapacity = db.Enclosures.Where(x => x.Id == EnclosureId).FirstOrDefault()?.MaxCapacity ?? 0;
        var count = 0;


        res.ForEach(animal =>
        {
            count = db.Animals.Where(x => x.EnclosureId == EnclosureId).Count();
            if (maxCapacity == count)
            {
                EnclosureId++;
                maxCapacity = db.Enclosures.Where(x => x.Id == EnclosureId).FirstOrDefault()?.MaxCapacity ?? 0;
            }

            animal.EnclosureId = EnclosureId;
            db.Animals.Add(animal);
            db.SaveChanges();
        });
    }
}