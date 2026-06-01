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

        // POST api/vehiculo
        [HttpPost]
        public async Task<ActionResult> Post(Vehiculo vehiculo)
        {
            // Verificar que el producto existe
            var producto = await _context.Productos.FindAsync(vehiculo.CveProducto);

            if (producto is null)
                return NotFound("Producto no encontrado");

            // Verificar que el producto no tenga ya un vehiculo o inmueble
            bool yaExiste = await _context.Vehiculos
                .AnyAsync(v => v.CveProducto == vehiculo.CveProducto);

            if (yaExiste)
                return BadRequest("Este producto ya tiene un vehículo asociado");

            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByProducto), new { idProducto = vehiculo.CveProducto }, vehiculo);
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