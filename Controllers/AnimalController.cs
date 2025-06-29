using System;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SQLitePCL;
using Zoo.Helpers;

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
        || animal.DateofBirth == DateOnly.MinValue
        || animal.SpeciesId == 0
        || animal.EnclosureId == 0)
        {
            return ValidationProblem("Not all required information is provided.");
        }
        if (animal.Sex != "male" && animal.Sex != "female")
        {
            return ValidationProblem("Invalid sex.");
        }
        if (animal.DateofAcquisition > DateOnly.FromDateTime(DateTime.Today)
        || animal.DateofBirth > DateOnly.FromDateTime(DateTime.Today))
        {
            return ValidationProblem("Invalid Date: Date of Acquisition or Date of Birth is in the future.");
        }

        var enclosure = _db.Enclosures.Where(x => x.Id == animal.EnclosureId);
        var species = _db.AllSpecies.Where(x => x.Id == animal.SpeciesId);
        var animals = _db.Animals.Where(x => x.EnclosureId == animal.EnclosureId);
       
        if (!species.Any() || !enclosure.Any())
        {
            return ValidationProblem("No such species or enclosure.");
        }
        if (enclosure.FirstOrDefault()?.MaxCapacity == animals.Count())
        {
            return ValidationProblem($"The enclosure, {animal.EnclosureId} is full.");
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
    
    [HttpGet("Search")]

  public IActionResult Search([FromQuery] SearchRequest searchRequest)
    {

        var speciesWithClassification = from species in _db.AllSpecies
                                        join classification in _db.Classifications
                                        on species.ClassificationId equals classification.Id
                                        select new
                                        {
                                            SpeciesId = species.Id,
                                            SpeciesName = species.Name,
                                            classification = classification.Name
                                        };

        var query = from animal in _db.Animals
                    join species in speciesWithClassification
                    on animal.SpeciesId equals species.SpeciesId
                    join enclosure in _db.Enclosures
                    on animal.EnclosureId equals enclosure.Id
                    select new
                    {
                        animal.Id,
                        animal.Name,
                        animal.Sex,
                        animal.DateofAcquisition,
                        animal.DateofBirth,
                        animal.EnclosureId,
                        animal.SpeciesId,
                        animal.Age,
                        SpeciesName = species.SpeciesName,
                        classification = species.classification,
                        EnclosureName = enclosure.Name
                };

        if (searchRequest.search != null)
        {
            if (!string.IsNullOrEmpty(searchRequest.search.Name))
            {
                query = query.Where(animal => animal.Name.ToLower().Contains(searchRequest.search.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchRequest.search.Species))
            {
                var species = _db.AllSpecies.FirstOrDefault(species => species.Name.ToLower().Contains(searchRequest.search.Species.ToLower()));
                if (species != null)
                {
                    query = query.Where(animal => animal.SpeciesId == species.Id);
                }
            }

            if (!string.IsNullOrEmpty(searchRequest.search.Enclosure))
            {
                var enclosure = _db.Enclosures.FirstOrDefault(enclosure => enclosure.Name.ToLower().Contains(searchRequest.search.Enclosure.ToLower()));
                if (enclosure != null)
                {
                    query = query.Where(animal => animal.EnclosureId == enclosure.Id);
                }
            }

            if (!string.IsNullOrEmpty(searchRequest.search.Classification))

            {
                var classification = _db.Classifications.FirstOrDefault(classification => classification.Name.ToLower().Contains(searchRequest.search.Classification.ToLower()));
                if (classification != null)
                {
                    var speciesIds = _db.AllSpecies
                                    .Where(s => s.ClassificationId == classification.Id)
                                    .Select(s => s.Id)
                                    .ToList();

                    // Filter the query by species IDs
                    query = query.Where(animal => speciesIds.Contains(animal.SpeciesId));

                }
            }

            if (searchRequest.search.Age.HasValue)

            {
                query = query.Where(animal => animal.Age == searchRequest.search.Age);
            }
            if (searchRequest.search.DateOfacquisition.HasValue)

            {
                query = query.Where(animal => animal.DateofAcquisition == searchRequest.search.DateOfacquisition);

            }
        }
    
        // Dynamic sorting
        query = searchRequest.SortBy.ToLower() switch
        {
            "name" => query.OrderBy(a => a.Name),
            "age" => query.OrderBy(a => a.Age),
            "species" => query.OrderBy(a => a.SpeciesName),
            "enclosure" => query.OrderBy(a => a.EnclosureName),
            "classification" => query.OrderBy(a => a.classification),
            "dateofacquisition" => query.OrderBy(a => a.DateofAcquisition),
            _ => query.OrderBy(a => a.SpeciesName) // Default sorting by Id
        };

        var totalCount = query.Count();

        var totalPages = (int)Math.Ceiling((double)totalCount / searchRequest.PageSize);

        query = query.Skip((searchRequest.Page - 1) * searchRequest.PageSize).Take(searchRequest.PageSize);

        var result = new

        {

            TotalCount = totalCount,

            TotalPages = totalPages,

            CurrentPage = searchRequest.Page,

            PageSize = searchRequest.PageSize,

            Animals = query.ToList()

        };
        return Ok(result);

    }
}

