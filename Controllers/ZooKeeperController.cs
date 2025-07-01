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
public class ZooKeeperController : ControllerBase
{

    private readonly ILogger<AnimalController> _logger;
    private readonly ZooDbContext _db;

    public ZooKeeperController(
        ILogger<AnimalController> logger,
        ZooDbContext db)
    {
        _logger = logger;
        _db = db;
    }


    [HttpGet("GetZooKeeperbyId/{Id}")]
    public IEnumerable<ZooKeeperEnclosure> Get(int Id)
    {
        var zooKeeperEnclosures = _db.ZooKeeperEnclosures
                                .Include(a => a.zookeeper)
                                .Include(a => a.enclosure)
                                .Where(x => x.ZooKeeperId == Id)
                                .ToList();
        return zooKeeperEnclosures;
    }

    [HttpPost("CreateZooKeeper")]
    public ActionResult CreateZooKeeper([FromBody] CreateZooKeeperRequest request)
    {
        
        if (string.IsNullOrEmpty(request.Name)
        || request.Enclosures == null)
        {
            return ValidationProblem("Not all required information is provided.");
        }
        if (request.Enclosures.Count()<=0)
        {
            return ValidationProblem("No enclosure is assigned.");
        }

        var newZooKeeper = new ZooKeeper
        {
            Name = request.Name
        };

        var id = 0;
        var addZooKeeper = false;

        foreach (var enclosure in request.Enclosures)
        {
            if (!int.TryParse(enclosure.ToString(), out id))
            {
                return ValidationProblem($"Enclosure, {enclosure}, is not a valid integer. Please try again.");
            }
            if (!_db.Enclosures.Any(x => x.Id == id))
            {
                return ValidationProblem($"Enclosure, {enclosure}, doesn't exist. Please try again.");
            }
            if (!addZooKeeper)
            {
                _db.ZooKeepers.Add(newZooKeeper);
                _db.SaveChanges();
                addZooKeeper = true;
            }
            _db.ZooKeeperEnclosures.Add(new ZooKeeperEnclosure
            {
                ZooKeeperId = newZooKeeper.Id,
                EnclosureId = id
            });
            _db.SaveChanges();
        }

        return Ok($"Zookeeper {request.Name} is added.");
    }
}
