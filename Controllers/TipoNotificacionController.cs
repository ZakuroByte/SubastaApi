using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoNotificacionController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public TipoNotificacionController(SubastaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoNotificacion>>> Get()
        {
            return await _context.TiposNotificacion.ToListAsync();
        }
    }
}