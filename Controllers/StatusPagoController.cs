using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusPagoController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public StatusPagoController(SubastaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusPago>>> Get()
        {
            return await _context.StatusPagos.ToListAsync();
        }
    }
}