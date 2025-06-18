using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Zoo.Controllers;

[ApiController]
[Route("[controller]")]
public class AnimalController : ControllerBase
{

    private readonly ILogger<AnimalController> _logger;
    private readonly ZooDbContext _db;

    public AnimalController(
        ILogger<AnimalController> logger,
        ZooDbContext db)
    {
        _logger = logger;
        _db = db;
    }


    [HttpGet("GetAnimalbyId/{Id}")]
    public IEnumerable<Animal> Get(int Id)
    {
        var animal = _db.Animals.Where(x => x.Id == Id);
        return animal;
    }

    [HttpPost("AddAnimal")]
    public ActionResult AddAnimal(Animal animal)
    {
        if (animal.Name == null
        || animal.Sex == null
        || animal.DateofAcquisition == DateOnly.MinValue
        || animal.DateofBirth == DateOnly.MinValue)
        {
            return ValidationProblem("Input information is invalid.");
        }
        _db.Animals.Add(new Animal
        {
            Name = animal.Name,
            Sex = animal.Sex,
            DateofAcquisition = animal.DateofAcquisition,
            DateofBirth = animal.DateofBirth,
            SpeciesId = animal.SpeciesId,
            EnclosureId = animal.EnclosureId
        });
        _db.SaveChanges();
        return Ok($"Animal {animal.Name} is added.");
    }

    [HttpGet("GetSpeciesInZoo")]
    public IEnumerable<TypeOfAnimal> GetSpeciesInZoo()
    {
        var speciesWithClassification = from species in _db.AllSpecies
                                         join classification in _db.Classifications
                                         on species.ClassificationId equals classification.Id
                                         select new
                                         {
                                             SpeciesId = species.Id,
                                             SpeciesName = species.Name,
                                             Classification = classification.Name
                                         };

        var result = (from animal in _db.Animals
                    join species in speciesWithClassification
                    on animal.SpeciesId equals species.SpeciesId
                    group species by new { species.SpeciesId, species.SpeciesName, species.Classification } into grouped
                    select new TypeOfAnimal
                    {
                      SpeciesName = grouped.Key.SpeciesName,
                      Classification = grouped.Key.Classification
                    }).ToList();

        return result;
    }
    
}

