using Microsoft.EntityFrameworkCore;
using SubastaApi.Entidades;

namespace SubastaApi.Services
{
    public class PagoVencidoService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PagoVencidoService> _logger;

        public PagoVencidoService(IServiceScopeFactory scopeFactory, ILogger<PagoVencidoService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de pagos vencidos iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcesarPagosVencidos();
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }

        private async Task ProcesarPagosVencidos()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Data.SubastaDbContext>();

            var ahora = DateTime.UtcNow;

            // Traer pagos pendientes cuyo tiempo límite ya pasó
            var pagosVencidos = await context.Pagos
                .Include(p => p.SubastaRef)
                    .ThenInclude(s => s!.ProductoRef)
                .Where(p => p.CveStatusPago == 1 && p.FechaLimite <= ahora)
                .ToListAsync();

            foreach (var pago in pagosVencidos)
            {
                // Marcar pago como vencido
                pago.CveStatusPago = 3; // Vencido
                context.Update(pago);

                // Regresar el producto a Disponible
                if (pago.SubastaRef?.ProductoRef != null)
                {
                    pago.SubastaRef.ProductoRef.CveStatusProducto = 1; // Disponible
                    context.Update(pago.SubastaRef.ProductoRef);
                }

                // Notificar al comprador que su pago venció
                if (pago.SubastaRef?.CveUsuarioGanador != null)
                {
                    context.Notificaciones.Add(new Notificacion
                    {
                        Descripcion = "Tu tiempo límite de pago ha vencido. El producto ha sido liberado.",
                        FechaEnvio = ahora,
                        Leida = false,
                        CveUsuario = pago.SubastaRef.CveUsuarioGanador.Value,
                        CveTipoNotificacion = 3,
                        CveSubasta = pago.CveSubasta
                    });
                }

                // Notificar al vendedor que el pago venció
                if (pago.SubastaRef?.ProductoRef != null)
                {
                    context.Notificaciones.Add(new Notificacion
                    {
                        Descripcion = "El comprador no realizó el pago a tiempo. Tu producto está disponible nuevamente.",
                        FechaEnvio = ahora,
                        Leida = false,
                        CveUsuario = pago.SubastaRef.ProductoRef.CveUsuario,
                        CveTipoNotificacion = 3,
                        CveSubasta = pago.CveSubasta
                    });
                }

                _logger.LogInformation($"Pago {pago.IdPago} marcado como vencido");
            }

            await context.SaveChangesAsync();
        }
    }
}