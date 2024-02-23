using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Numerics;
using ZohoAPI.Models;

namespace ZohoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrucksController : Controller
    { 
    private readonly AppSettingsDbContext _context;

    public TrucksController(AppSettingsDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public JsonResult Get()
    {
            var result = _context.Trucks.ToList();
            return new JsonResult(Ok(result));
    }
    [HttpGet("{id:int}")]
    public async Task<Trucks> Get(int Id)
    {
        return await _context.Trucks.FirstAsync(s => s.Id == Id);
    }
    [HttpPost]
    public JsonResult Post(Trucks truck)
        {
            if(truck.Id == 0)
            {
                _context.Trucks.Add(truck);
            }
            else
            {
                var truckInDb = _context.Trucks.Find(truck.Id);
                if (truckInDb == null)
                    return new JsonResult(NotFound());
                truckInDb = truck;
            }
            _context.SaveChanges();
            return new JsonResult(Ok(truck));
        }
    }
}

