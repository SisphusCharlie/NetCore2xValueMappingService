using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValueMappingCoreAPI.Areas.APIArea.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ValueMappingCoreAPI.Areas.APIArea.Controllers
{
    [Route("APIArea/ValueMap")]
    [FormatFilter]
    public class ValueMapController : Controller
    {
        private readonly ValueMapContext _context;

        public ValueMapController(ValueMapContext context)
        {
            _context = context;
        }

        // GET: api/ValueMaps
        [HttpGet]
        public IEnumerable<ValueMaps> GetValueMaps()
        {
            return _context.ValueMaps;
        }

        [HttpGet("Binary")]
        [Produces("application/proto")]
        public IEnumerable<ValueMaps> GetBinaryValueMaps()
        {
            return _context.ValueMaps;
        }
        [HttpGet("xml")]
        [Produces("application/xml")]
        public IEnumerable<ValueMaps> GetBinaryValueMapsxml()
        {
            return _context.ValueMaps;
        }

        [HttpGet("{id}/Content")]
        public ContentResult GetContent([FromRoute] int id)
        {
            return Content(_context.ValueMaps.Where((x) => x.Id == id).FirstOrDefault().ValuationFunction.ToString());
        }


        [Route("[Controller]/[action]/{id}.{format?}")]
        public ValueMaps GetContentByID(int id)
        {
            return _context.ValueMaps.Where((x) => x.Id == id).FirstOrDefault();
        }



        // GET: api/ValueMaps/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValueMaps([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var valueMaps = await _context.ValueMaps.SingleOrDefaultAsync(m => m.Id == id);

            if (valueMaps == null)
            {
                return NotFound();
            }

            return Ok(valueMaps);
        }

        // PUT: api/ValueMaps/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutValueMaps([FromRoute] int id, [FromBody] ValueMaps valueMaps)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != valueMaps.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(valueMaps).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ValueMapExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/ValueMaps
        //[HttpPost]
        //public async Task<IActionResult> PostValueMaps([FromBody] ValueMaps valueMaps)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.ValueMaps.Add(valueMaps);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetValueMaps", new { id = valueMaps.Id }, valueMaps);
        //}

        // DELETE: api/ValueMaps/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteValueMaps([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var valueMaps = await _context.ValueMaps.SingleOrDefaultAsync(m => m.Id == id);
        //    if (valueMaps == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ValueMaps.Remove(valueMaps);
        //    await _context.SaveChangesAsync();

        //    return Ok(valueMaps);
        //}

        //private bool ValueMapsExists(int id)
        //{
        //    return _context.ValueMaps.Any(e => e.Id == id);
        //}
    }
}
