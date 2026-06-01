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
    public class FotoProductoController : ControllerBase
    {
        private readonly SubastaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FotoProductoController(SubastaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET api/fotoproducto/producto/1
        [AllowAnonymous]
        [HttpGet("producto/{idProducto:int}")]
        public async Task<ActionResult<IEnumerable<FotoProducto>>> GetByProducto(int idProducto)
        {
            var fotos = await _context.FotosProducto
                .Where(f => f.CveProducto == idProducto)
                .ToListAsync();

            return Ok(fotos);
        }

        // POST api/fotoproducto
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] int idProducto, [FromForm] List<IFormFile> fotos)
        {
            // Verificar que el producto existe
            var producto = await _context.Productos.FindAsync(idProducto);

            if (producto is null)
                return NotFound("Producto no encontrado");

            if (fotos == null || fotos.Count == 0)
                return BadRequest("No se enviaron fotos");

            // Carpeta donde se guardarán las fotos
            var carpeta = Path.Combine(_env.WebRootPath, "fotos", idProducto.ToString());

            // Crear la carpeta si no existe
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            var fotosGuardadas = new List<FotoProducto>();

            foreach (var foto in fotos)
            {
                // Validar que sea una imagen
                var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(foto.FileName).ToLower();

                if (!extensionesPermitidas.Contains(extension))
                    return BadRequest($"El archivo {foto.FileName} no es una imagen válida");

                // Validar tamaño máximo 5MB
                if (foto.Length > 5 * 1024 * 1024)
                    return BadRequest($"El archivo {foto.FileName} supera el tamaño máximo de 5MB");

                // Generar nombre único para evitar sobreescrituras
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                // Guardar el archivo físicamente
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

                // Guardar la URL en la base de datos
                var fotoProducto = new FotoProducto
                {
                    Url = $"/fotos/{idProducto}/{nombreArchivo}",
                    CveProducto = idProducto
                };

                fotosGuardadas.Add(fotoProducto);
            }

            _context.FotosProducto.AddRange(fotosGuardadas);
            await _context.SaveChangesAsync();

            return Ok(fotosGuardadas);
        }

        // DELETE api/fotoproducto/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var foto = await _context.FotosProducto.FindAsync(id);

            if (foto is null)
                return NotFound();

            // Borrar el archivo físico
            var rutaArchivo = Path.Combine(_env.WebRootPath, foto.Url.TrimStart('/'));

            if (System.IO.File.Exists(rutaArchivo))
                System.IO.File.Delete(rutaArchivo);

            _context.FotosProducto.Remove(foto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}