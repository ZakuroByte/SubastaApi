using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CondicionController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public CondicionController(SubastaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Condicion>>> Get()
        {
            return await _context.Condiciones.ToListAsync();
        }
    }
}