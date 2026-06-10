using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusSubastaController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public StatusSubastaController(SubastaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusSubasta>>> Get()
        {
            return await _context.StatusSubastas.ToListAsync();
        }
    }
}