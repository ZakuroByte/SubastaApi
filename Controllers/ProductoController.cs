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
    public class ProductoController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public ProductoController(SubastaDbContext context)
        {
            _context = context;
        }

        // GET api/producto
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            var productos = await _context.Productos
                .Include(p => p.CategoriaRef)
                .Include(p => p.CondicionRef)
                .Include(p => p.UsuarioRef)
                .Include(p => p.StatusProductoRef)
                .Include(p => p.Fotos)
                .ToListAsync();

            return Ok(productos);
        }

        // GET api/producto/1
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Producto>> Get(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.CategoriaRef)
                .Include(p => p.CondicionRef)
                .Include(p => p.UsuarioRef)
                .Include(p => p.StatusProductoRef)
                .Include(p => p.Fotos)
                .Include(p => p.VehiculoRef)
                .Include(p => p.InmuebleRef)
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto is null)
                return NotFound();

            return Ok(producto);
        }

        // GET api/producto/usuario/1
        [AllowAnonymous]
        [HttpGet("usuario/{idUsuario:int}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetByUsuario(int idUsuario)
        {
            var productos = await _context.Productos
                .Include(p => p.CategoriaRef)
                .Include(p => p.CondicionRef)
                .Include(p => p.StatusProductoRef)
                .Include(p => p.Fotos)
                .Where(p => p.CveUsuario == idUsuario)
                .ToListAsync();

            return Ok(productos);
        }

        // POST api/producto
        [HttpPost]
        public async Task<ActionResult> Post(Producto producto)
        {
            // El status inicial siempre es Disponible al crear
            producto.CveStatusProducto = 1;

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = producto.IdProducto }, producto);
        }

        // PUT api/producto/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Producto producto)
        {
            if (id != producto.IdProducto)
                return BadRequest("Los ids deben coincidir");

            // No permitir editar si ya está en subasta o vendido
            var productoDb = await _context.Productos.FindAsync(id);

            if (productoDb is null)
                return NotFound();

            if (productoDb.CveStatusProducto != 1)
                return BadRequest("Solo se puede editar un producto con status Disponible");

            _context.Update(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT api/producto/1/status
        [HttpPut("{id:int}/status")]
        public async Task<ActionResult> PutStatus(int id, [FromBody] int idStatus)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto is null)
                return NotFound();

            producto.CveStatusProducto = idStatus;

            _context.Update(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}