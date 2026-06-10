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
    public class VehiculoController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public VehiculoController(SubastaDbContext context)
        {
            _context = context;
        }

        // GET api/vehiculo/producto/1
        [AllowAnonymous]
        [HttpGet("producto/{idProducto:int}")]
        public async Task<ActionResult<Vehiculo>> GetByProducto(int idProducto)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.ProductoRef)
                .FirstOrDefaultAsync(v => v.CveProducto == idProducto);

            if (vehiculo is null)
                return NotFound();

            return Ok(vehiculo);
        }

        // PUT api/vehiculo/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.IdVehiculo)
                return BadRequest("Los ids deben coincidir");

            var vehiculoDb = await _context.Vehiculos.FindAsync(id);

            if (vehiculoDb is null)
                return NotFound();

            _context.Update(vehiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}