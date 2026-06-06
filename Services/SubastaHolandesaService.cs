using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;

namespace SubastaApi.Services
{
    public class SubastaHolandesaService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SubastaHolandesaService> _logger;

        public SubastaHolandesaService(IServiceScopeFactory scopeFactory, ILogger<SubastaHolandesaService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de subasta holandesa iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcesarSubastasHolandesas();

                // Esperar 1 minuto antes de volver a revisar
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcesarSubastasHolandesas()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SubastaDbContext>();

            // Traer todas las subastas holandesas activas
            var subastas = await context.Subastas
                .Where(s => s.CveTipoSubasta == 2 && s.CveStatusSubasta == 2)
                .ToListAsync();

            foreach (var subasta in subastas)
            {
                var ahora = DateTime.UtcNow;

                // Verificar si ya pasó el tiempo de la subasta
                if (ahora >= subasta.FechaFinal)
                {
                    // Notificar al vendedor que nadie compró
                    var notificacion = new Entidades.Notificacion
                    {
                        Descripcion = "Tu subasta holandesa terminó sin compradores. Puedes decidir qué hacer con el producto.",
                        FechaEnvio = ahora,
                        Leida = false,
                        CveTipoNotificacion = 3,
                        CveSubasta = subasta.IdSubasta,
                        CveUsuario = await context.Productos
                            .Where(p => p.IdProducto == subasta.CveProducto)
                            .Select(p => p.CveUsuario)
                            .FirstOrDefaultAsync()
                    };

                    context.Notificaciones.Add(notificacion);

                    // Cambiar status a Finalizada sin ganador
                    subasta.CveStatusSubasta = 3;
                    context.Update(subasta);

                    _logger.LogInformation($"Subasta holandesa {subasta.IdSubasta} finalizada sin ganador");
                    continue;
                }

                // Calcular cuántos intervalos han pasado desde el inicio
                var minutosTranscurridos = (ahora - subasta.FechaInicio).TotalMinutes;
                var intervalosTranscurridos = (int)(minutosTranscurridos / subasta.IntervaloMinutos);

                // Calcular el precio que debería tener ahora
                var factorDecremento = 1 - (subasta.Decremento / 100);
                var precioCalculado = subasta.PrecioInicial * (decimal)Math.Pow((double)factorDecremento, intervalosTranscurridos);

                // Respetar el precio mínimo (PrecioActual se usa como mínimo en holandesa al crear)
                var precioMinimo = subasta.PrecioActual;
                var nuevoPrecio = Math.Max(precioCalculado, precioMinimo);

                // Solo actualizar si el precio cambió
                if (nuevoPrecio != subasta.PrecioActual)
                {
                    _logger.LogInformation($"Subasta {subasta.IdSubasta}: precio actualizado de {subasta.PrecioActual} a {nuevoPrecio}");
                    subasta.PrecioActual = nuevoPrecio;
                    context.Update(subasta);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}