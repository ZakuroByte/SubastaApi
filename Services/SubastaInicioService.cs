using Microsoft.EntityFrameworkCore;

namespace SubastaApi.Services
{
    public class SubastaInicioService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SubastaInicioService> _logger;

        public SubastaInicioService(IServiceScopeFactory scopeFactory, ILogger<SubastaInicioService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de inicio de subastas iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                await IniciarSubastas();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task IniciarSubastas()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Data.SubastaDbContext>();

            var ahora = DateTime.UtcNow;

            // Traer subastas pendientes cuya fecha de inicio ya llegó
            var subastas = await context.Subastas
                .Include(s => s.ProductoRef)
                .Where(s => s.CveStatusSubasta == 1 && s.FechaInicio <= ahora)
                .ToListAsync();

            foreach (var subasta in subastas)
            {
                subasta.CveStatusSubasta = 2; // Activa
                context.Update(subasta);

                // Notificar al vendedor que su subasta inició
                context.Notificaciones.Add(new Entidades.Notificacion
                {
                    Descripcion = "Tu subasta ha iniciado y ya está disponible para recibir ofertas.",
                    FechaEnvio = ahora,
                    Leida = false,
                    CveUsuario = subasta.ProductoRef!.CveUsuario,
                    CveTipoNotificacion = 1,
                    CveSubasta = subasta.IdSubasta
                });

                _logger.LogInformation($"Subasta {subasta.IdSubasta} iniciada automáticamente");
            }

            await context.SaveChangesAsync();
        }
    }
}