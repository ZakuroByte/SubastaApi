using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InmuebleController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public InmuebleController(SubastaDbContext context)
        {
            _context = context;
        }

        // GET api/inmueble/producto/1
        [AllowAnonymous]
        [HttpGet("producto/{idProducto:int}")]
        public async Task<ActionResult<Inmueble>> GetByProducto(int idProducto)
        {
            var inmueble = await _context.Inmuebles
                .Include(i => i.ProductoRef)
                .FirstOrDefaultAsync(i => i.CveProducto == idProducto);

            if (inmueble is null)
                return NotFound();

            return Ok(inmueble);
        }

        // PUT api/inmueble/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Inmueble inmueble)
        {
            if (id != inmueble.IdInmueble)
                return BadRequest("Los ids deben coincidir");

            var inmuebleDb = await _context.Inmuebles.FindAsync(id);

            if (inmuebleDb is null)
                return NotFound();

            _context.Update(inmueble);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}