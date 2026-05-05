using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusProductoController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public StatusProductoController(SubastaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusProducto>>> Get()
        {
            return await _context.StatusProductos.ToListAsync();
        }
    }
}