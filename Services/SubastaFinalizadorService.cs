using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Services
{
    public class SubastaFinalizadorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SubastaFinalizadorService> _logger;

        public SubastaFinalizadorService(IServiceScopeFactory scopeFactory, ILogger<SubastaFinalizadorService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio finalizador de subastas iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                await FinalizarSubastas();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task FinalizarSubastas()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SubastaDbContext>();

            var ahora = DateTime.UtcNow;

            // Traer subastas activas cuyo tiempo ya terminó
            // Inglesa (1), Sellada (3) — Holandesa la maneja el controlador de Oferta
            var subastas = await context.Subastas
                .Include(s => s.Ofertas)
                .Include(s => s.ProductoRef)
                .Where(s =>
                    s.CveStatusSubasta == 2 &&
                    s.FechaFinal <= ahora &&
                    (s.CveTipoSubasta == 1 || s.CveTipoSubasta == 3))
                .ToListAsync();

            foreach (var subasta in subastas)
            {
                var ofertaGanadora = subasta.Ofertas
                    .OrderByDescending(o => o.Monto)
                    .FirstOrDefault();

                if (ofertaGanadora != null)
                {
                    // Hay ganador
                    subasta.CveUsuarioGanador = ofertaGanadora.CveUsuario;
                    subasta.CveStatusSubasta = 3; // Finalizada
                    subasta.ProductoRef!.CveStatusProducto = 3; // Vendido

                    // Calcular tiempo límite de pago según tipo de producto
                    int horasPago = 48;

                    bool esVehiculo = await context.Vehiculos
                        .AnyAsync(v => v.CveProducto == subasta.CveProducto);

                    bool esInmueble = await context.Inmuebles
                        .AnyAsync(i => i.CveProducto == subasta.CveProducto);

                    if (esVehiculo) horasPago = 72;
                    if (esInmueble) horasPago = 168;

                    // Crear pago
                    var pago = new Pago
                    {
                        Monto = ofertaGanadora.Monto,
                        FechaRealizacion = ahora,
                        FechaLimite = ahora.AddHours(horasPago),
                        CveStatusPago = 1, // Pendiente
                        CveSubasta = subasta.IdSubasta
                    };

                    context.Pagos.Add(pago);

                    // Notificar al ganador
                    context.Notificaciones.Add(new Notificacion
                    {
                        Descripcion = $"¡Ganaste la subasta! Tienes {horasPago} horas para realizar el pago.",
                        FechaEnvio = ahora,
                        Leida = false,
                        CveUsuario = ofertaGanadora.CveUsuario,
                        CveTipoNotificacion = 3, // Pago Pendiente
                        CveSubasta = subasta.IdSubasta
                    });

                    // Notificar al vendedor
                    context.Notificaciones.Add(new Notificacion
                    {
                        Descripcion = $"Tu subasta ha finalizado. El ganador tiene {horasPago} horas para realizar el pago.",
                        FechaEnvio = ahora,
                        Leida = false,
                        CveUsuario = subasta.ProductoRef!.CveUsuario,
                        CveTipoNotificacion = 3,
                        CveSubasta = subasta.IdSubasta
                    });

                    _logger.LogInformation($"Subasta {subasta.IdSubasta} finalizada con ganador {ofertaGanadora.CveUsuario}");
                }
                else
                {
                    // Sin ofertas, finaliza sin ganador
                    subasta.CveStatusSubasta = 3;
                    subasta.ProductoRef!.CveStatusProducto = 1; // Regresa a Disponible

                    // Notificar al vendedor
                    context.Notificaciones.Add(new Notificacion
                    {
                        Descripcion = "Tu subasta finalizó sin ofertas. El producto está disponible nuevamente.",
                        FechaEnvio = ahora,
                        Leida = false,
                        CveUsuario = subasta.ProductoRef!.CveUsuario,
                        CveTipoNotificacion = 3,
                        CveSubasta = subasta.IdSubasta
                    });

                    _logger.LogInformation($"Subasta {subasta.IdSubasta} finalizada sin ganador");
                }

                context.Update(subasta);
            }

            await context.SaveChangesAsync();
        }
    }
}