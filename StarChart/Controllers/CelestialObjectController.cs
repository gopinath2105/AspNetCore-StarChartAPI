using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext _context1)
        {
            this._context = _context1;
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Where(c => c.Id == id).SingleOrDefault();
            List<CelestialObject> finalResult = new List<CelestialObject>();
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                List<CelestialObject> obj = new List<CelestialObject>();
                obj.Add(_context.CelestialObjects.Where(c => c.OrbitedObjectId == id).SingleOrDefault());
                result.Satellites = obj;
            }
            return Ok(result);

        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(c => c.Name == name);

            List<CelestialObject> FinalResult = new List<CelestialObject>();
            if (result.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                foreach (var item in result)
                {
                    item.Satellites = new List<CelestialObject>();
                    item.Satellites.Add(item);
                    FinalResult.Add(item);
                }
                return Ok(FinalResult);
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> result = new List<CelestialObject>();
            CelestialObject obj = new CelestialObject();
            obj.Id = 5;
            obj.Name = "Gopinath";
            this.Create(obj);
            foreach (var item in _context.CelestialObjects)
            {
                {
                    item.Satellites = new List<CelestialObject>();
                    item.Satellites.Add(item);
                    result.Add(item);
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            if (celestialObject != null && celestialObject.Satellites == null)
            {
                celestialObject.Satellites = new List<CelestialObject>();
            }
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            {
                var result = _context.CelestialObjects.SingleOrDefault(b => b.Id == id);


                if (result != null)
                {
                    result.Name = celestialObject.Name;
                    result.OrbitalPeriod = celestialObject.OrbitalPeriod;
                    result.OrbitedObjectId = celestialObject.OrbitedObjectId;
                    result.Satellites = celestialObject.Satellites;
                    _context.SaveChanges();
                    return NoContent();
                }
                else
                    return NotFound();
            }
        }


        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            {
                var result = _context.CelestialObjects.SingleOrDefault(b => b.Id == id);


                if (result != null)
                {
                    result.Name = name;
                    _context.SaveChanges();
                    return NoContent();
                }
                else
                    return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            {
                var result = _context.CelestialObjects.SingleOrDefault(b => b.Id == id);


                if (result != null)
                {
                    _context.CelestialObjects.Remove(result);
                    _context.SaveChanges();
                    return NoContent();
                }
                else
                    return NotFound();
            }
        }
    }
}
