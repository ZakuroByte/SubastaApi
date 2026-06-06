using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SubastaApi.Data;
using SubastaApi.Entidades;
using SubastaApi.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SubastaDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SubastaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.TipoUsuarioRef)
                .FirstOrDefaultAsync(u => u.Correo == loginDto.Correo);

            if (usuario == null)
                return Unauthorized("Credenciales incorrectas");

            bool passwordValida = BCrypt.Net.BCrypt.Verify(loginDto.Contrasenia, usuario.Contrasenia);

            if (!passwordValida)
                return Unauthorized("Credenciales incorrectas");

            string token = GenerarToken(usuario);

            return Ok(new
            {
                token,
                usuario = new UsuarioRespuestaDto
                {
                    IdUsuario = usuario.IdUsuario,
                    Correo = usuario.Correo,
                    Nombre = usuario.Nombre,
                    ApellidoPaterno = usuario.ApellidoPaterno,
                    ApellidoMaterno = usuario.ApellidoMaterno,
                    Calificacion = usuario.Calificacion,
                    CveTipoUsuario = usuario.CveTipoUsuario,
                    TipoUsuario = usuario.TipoUsuarioRef?.Descripcion ?? ""
                }
            });
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] Usuario usuario)
        {
            bool correoExiste = await _context.Usuarios
                .AnyAsync(u => u.Correo == usuario.Correo);

            if (correoExiste)
                return BadRequest("El correo ya está registrado");

            usuario.Contrasenia = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenia);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Cargar el tipo de usuario para la respuesta
            await _context.Entry(usuario)
                .Reference(u => u.TipoUsuarioRef)
                .LoadAsync();

            string token = GenerarToken(usuario);

            return Ok(new
            {
                token,
                usuario = new UsuarioRespuestaDto
                {
                    IdUsuario = usuario.IdUsuario,
                    Correo = usuario.Correo,
                    Nombre = usuario.Nombre,
                    ApellidoPaterno = usuario.ApellidoPaterno,
                    ApellidoMaterno = usuario.ApellidoMaterno,
                    Calificacion = usuario.Calificacion,
                    CveTipoUsuario = usuario.CveTipoUsuario,
                    TipoUsuario = usuario.TipoUsuarioRef?.Descripcion ?? ""
                }
            });
        }

        private string GenerarToken(Usuario usuario)
        {
            // Claims — información que va dentro del token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.TipoUsuarioRef?.Descripcion ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8), // el token dura 8 horas
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}