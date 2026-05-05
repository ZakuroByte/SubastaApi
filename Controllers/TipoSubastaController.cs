using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoSubastaController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public TipoSubastaController(SubastaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoSubasta>>> Get()
        {
            return await _context.TiposSubasta.ToListAsync();
        }
    }
}