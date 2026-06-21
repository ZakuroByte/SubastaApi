import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

function DetallesUsuario() {
    const navigate = useNavigate()
    const [usuario, setUsuario] = useState(null);
    const [subastas, setSubastas] = useState([]);
    const [subastasGanadas, setSubastasGanadas] = useState([]);
    const [pagos, setPagos] = useState([]);
    const [tab, setTab] = useState('activas') // activas | historial | compras
    const [error, setError] = useState(null);

    const token = localStorage.getItem('token');
    const id = localStorage.getItem('idUsuario');

    useEffect(() => {
        if (!token || !id) { navigate('/'); return }

        // Usuario
        fetch(`http://localhost:5288/api/usuario/${id}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        })
            .then(r => r.json())
            .then(setUsuario)
            .catch(() => setError('No se pudo cargar la información del usuario'))

        // Subastas como vendedor
        fetch(`http://localhost:5288/api/subasta/vendedor/${id}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        })
            .then(r => r.json())
            .then(setSubastas)
            .catch(() => {})

        // Pagos (subastas ganadas como comprador)
        fetch(`http://localhost:5288/api/Pago/usuario/${id}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        })
            .then(r => r.json())
            .then(setPagos)
            .catch(() => {})
    }, []);

    const formatFecha = (f) => new Date(f).toLocaleDateString('es-MX', {
        day: '2-digit', month: 'short', year: 'numeric'
    })

    const formatMonto = (m) => `$${Number(m).toLocaleString('es-MX')}`

    const statusColor = (cve) => {
        if (cve === 1) return 'bg-gray-100 text-gray-600'
        if (cve === 2) return 'bg-green-100 text-green-700'
        if (cve === 3) return 'bg-blue-100 text-blue-700'
        if (cve === 4) return 'bg-red-100 text-red-600'
        return 'bg-gray-100 text-gray-600'
    }

    const statusLabel = (cve) => {
        if (cve === 1) return 'Pendiente'
        if (cve === 2) return 'Activa'
        if (cve === 3) return 'Finalizada'
        if (cve === 4) return 'Cancelada'
        return '—'
    }

    const statusPagoColor = (cve) => {
        if (cve === 1) return 'bg-yellow-100 text-yellow-700'
        if (cve === 2) return 'bg-green-100 text-green-700'
        if (cve === 3) return 'bg-red-100 text-red-600'
        return 'bg-gray-100 text-gray-600'
    }

    const statusPagoLabel = (cve) => {
        if (cve === 1) return 'Pago pendiente'
        if (cve === 2) return 'Pagado'
        if (cve === 3) return 'Vencido'
        return '—'
    }

    const esVendedor = usuario?.cveTipoUsuario === 2

    const subastasActivas = subastas.filter(s => s.cveStatusSubasta === 1 || s.cveStatusSubasta === 2)
    const subastasHistorial = subastas.filter(s => s.cveStatusSubasta === 3 || s.cveStatusSubasta === 4)

    const estrellas = (n) => '★'.repeat(n) + '☆'.repeat(5 - n)

    if (error) return <p className="text-center mt-10 text-red-500">{error}</p>
    if (!usuario) return <p className="text-center mt-10 text-gray-400">Cargando...</p>

    return (
        <div className="min-h-screen bg-gray-50 pt-20 pb-12">
            <div className="max-w-4xl mx-auto px-4">

                {/* Tarjeta de perfil */}
                <div className="bg-white rounded-2xl border border-gray-200 p-6 flex flex-col sm:flex-row items-center sm:items-start gap-5 mb-6">
                    <div className="w-20 h-20 bg-gray-100 rounded-full flex items-center justify-center flex-shrink-0">
                        <img src="/logo.png" alt="avatar" className="w-12 h-12 object-contain" />
                    </div>
                    <div className="flex-1 text-center sm:text-left">
                        <h2 className="text-2xl font-semibold text-gray-800">
                            {usuario.nombre} {usuario.apellidoPaterno} {usuario.apellidoMaterno}
                        </h2>
                        <p className="text-sm text-gray-400 mt-0.5">{usuario.correo}</p>
                        <div className="flex items-center justify-center sm:justify-start gap-2 mt-2">
                            <span className="text-yellow-400 text-lg">{estrellas(usuario.calificacion ?? 0)}</span>
                            <span className="text-sm text-gray-500">{usuario.calificacion ?? 0} / 5</span>
                        </div>
                        <span className={`inline-block mt-2 text-xs px-3 py-0.5 rounded-full font-medium ${esVendedor ? 'bg-blue-100 text-blue-700' : 'bg-purple-100 text-purple-700'}`}>
                            {esVendedor ? 'Vendedor' : 'Comprador'}
                        </span>
                    </div>

                    {/* Acciones */}
                    <div className="flex flex-col gap-2 w-full sm:w-auto">
                        <button
                            onClick={() => navigate('/actualizar-datos')}
                            className="bg-blue-600 text-white text-sm px-5 py-2 rounded-full hover:opacity-75 transition-all"
                        >
                            Actualizar datos
                        </button>
                        <button
                            onClick={() => navigate('/cambiar-contrasenia')}
                            className="bg-gray-100 text-gray-700 text-sm px-5 py-2 rounded-full hover:opacity-75 transition-all"
                        >
                            Cambiar contraseña
                        </button>
                    </div>
                </div>

                {/* Tabs */}
                <div className="flex gap-2 mb-4 border-b border-gray-200">
                    {esVendedor && (
                        <>
                            <button
                                onClick={() => setTab('activas')}
                                className={`px-4 py-2.5 text-sm font-medium border-b-2 transition-all ${tab === 'activas' ? 'border-blue-600 text-blue-600' : 'border-transparent text-gray-400 hover:text-gray-600'}`}
                            >
                                Subastas activas
                                {subastasActivas.length > 0 && (
                                    <span className="ml-2 bg-blue-100 text-blue-700 text-xs px-2 py-0.5 rounded-full">{subastasActivas.length}</span>
                                )}
                            </button>
                            <button
                                onClick={() => setTab('historial')}
                                className={`px-4 py-2.5 text-sm font-medium border-b-2 transition-all ${tab === 'historial' ? 'border-blue-600 text-blue-600' : 'border-transparent text-gray-400 hover:text-gray-600'}`}
                            >
                                Historial de ventas
                                {subastasHistorial.length > 0 && (
                                    <span className="ml-2 bg-gray-100 text-gray-600 text-xs px-2 py-0.5 rounded-full">{subastasHistorial.length}</span>
                                )}
                            </button>
                        </>
                    )}
                    <button
                        onClick={() => setTab('compras')}
                        className={`px-4 py-2.5 text-sm font-medium border-b-2 transition-all ${tab === 'compras' ? 'border-blue-600 text-blue-600' : 'border-transparent text-gray-400 hover:text-gray-600'}`}
                    >
                        Mis compras
                        {pagos.length > 0 && (
                            <span className="ml-2 bg-gray-100 text-gray-600 text-xs px-2 py-0.5 rounded-full">{pagos.length}</span>
                        )}
                    </button>
                </div>

                {/* Tab: Subastas activas (vendedor) */}
                {tab === 'activas' && esVendedor && (
                    <div>
                        {subastasActivas.length === 0 ? (
                            <div className="text-center py-16 text-gray-400">
                                <p className="text-3xl mb-2">🔨</p>
                                <p className="text-sm">No tienes subastas activas o pendientes</p>
                            </div>
                        ) : (
                            <div className="flex flex-col gap-3">
                                {subastasActivas.map(s => (
                                    <SubastaRow key={s.idSubasta} subasta={s} navigate={navigate}
                                        statusColor={statusColor} statusLabel={statusLabel} formatMonto={formatMonto} formatFecha={formatFecha} />
                                ))}
                            </div>
                        )}
                    </div>
                )}

                {/* Tab: Historial ventas (vendedor) */}
                {tab === 'historial' && esVendedor && (
                    <div>
                        {subastasHistorial.length === 0 ? (
                            <div className="text-center py-16 text-gray-400">
                                <p className="text-3xl mb-2">📋</p>
                                <p className="text-sm">No tienes subastas finalizadas o canceladas</p>
                            </div>
                        ) : (
                            <div className="flex flex-col gap-3">
                                {subastasHistorial.map(s => (
                                    <SubastaRow key={s.idSubasta} subasta={s} navigate={navigate}
                                        statusColor={statusColor} statusLabel={statusLabel} formatMonto={formatMonto} formatFecha={formatFecha} />
                                ))}
                            </div>
                        )}
                    </div>
                )}

                {/* Tab: Mis compras (comprador) */}
                {tab === 'compras' && (
                    <div>
                        {pagos.length === 0 ? (
                            <div className="text-center py-16 text-gray-400">
                                <p className="text-3xl mb-2">🛍️</p>
                                <p className="text-sm">Aún no has ganado ninguna subasta</p>
                            </div>
                        ) : (
                            <div className="flex flex-col gap-3">
                                {pagos.map(p => {
                                    const foto = p.subastaRef?.productoRef?.fotos?.[0]?.url
                                    const nombre = p.subastaRef?.productoRef?.nombre ?? 'Producto'
                                    const pagoPendiente = p.cveStatusPago === 1
                                    const vencido = p.cveStatusPago === 1 && new Date() > new Date(p.fechaLimite)

                                    return (
                                        <div key={p.idPago} className="bg-white rounded-2xl border border-gray-200 p-4 flex gap-4 items-center">
                                            {foto ? (
                                                <img src={`http://localhost:5288${foto}`} alt="" className="w-16 h-16 object-cover rounded-xl border border-gray-100 flex-shrink-0" />
                                            ) : (
                                                <div className="w-16 h-16 bg-gray-100 rounded-xl flex items-center justify-center text-2xl flex-shrink-0">📦</div>
                                            )}
                                            <div className="flex-1 min-w-0">
                                                <p className="font-medium text-gray-800 truncate">{nombre}</p>
                                                <p className="text-lg font-bold text-gray-900 mt-0.5">{formatMonto(p.monto)}</p>
                                                <div className="flex gap-2 items-center mt-1 flex-wrap">
                                                    <span className={`text-xs px-2 py-0.5 rounded-full font-medium ${vencido ? 'bg-red-100 text-red-600' : statusPagoColor(p.cveStatusPago)}`}>
                                                        {vencido ? 'Vencido' : statusPagoLabel(p.cveStatusPago)}
                                                    </span>
                                                    <span className="text-xs text-gray-400">
                                                        Límite: {formatFecha(p.fechaLimite)}
                                                    </span>
                                                </div>
                                            </div>
                                            <div className="flex flex-col gap-2 flex-shrink-0">
                                                {pagoPendiente && !vencido && (
                                                    <button
                                                        onClick={() => navigate(`/pago/${p.cveSubasta}`)}
                                                        className="bg-blue-600 text-white text-xs px-4 py-2 rounded-full hover:opacity-75 transition-all"
                                                    >
                                                        💳 Pagar
                                                    </button>
                                                )}
                                                {p.cveStatusPago === 2 && (
                                                    <button
                                                        onClick={() => navigate(`/pago/${p.cveSubasta}`)}
                                                        className="bg-yellow-400 text-yellow-900 text-xs px-4 py-2 rounded-full hover:opacity-75 transition-all"
                                                    >
                                                        ★ Calificar
                                                    </button>
                                                )}
                                                <button
                                                    onClick={() => navigate(`/subasta/${p.cveSubasta}`)}
                                                    className="bg-gray-100 text-gray-600 text-xs px-4 py-2 rounded-full hover:opacity-75 transition-all"
                                                >
                                                    Ver subasta
                                                </button>
                                            </div>
                                        </div>
                                    )
                                })}
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    )
}

function SubastaRow({ subasta, navigate, statusColor, statusLabel, formatMonto, formatFecha }) {
    const foto = subasta.productoRef?.fotos?.[0]?.url
    const nombre = subasta.productoRef?.nombre ?? 'Producto'
    const finalizada = subasta.cveStatusSubasta === 3

    return (
        <div className="bg-white rounded-2xl border border-gray-200 p-4 flex gap-4 items-center">
            {foto ? (
                <img src={`http://localhost:5288${foto}`} alt="" className="w-16 h-16 object-cover rounded-xl border border-gray-100 flex-shrink-0" />
            ) : (
                <div className="w-16 h-16 bg-gray-100 rounded-xl flex items-center justify-center text-2xl flex-shrink-0">📦</div>
            )}
            <div className="flex-1 min-w-0">
                <p className="font-medium text-gray-800 truncate">{nombre}</p>
                <p className="text-lg font-bold text-gray-900 mt-0.5">{formatMonto(subasta.precioActual)}</p>
                <div className="flex gap-2 items-center mt-1 flex-wrap">
                    <span className={`text-xs px-2 py-0.5 rounded-full font-medium ${statusColor(subasta.cveStatusSubasta)}`}>
                        {statusLabel(subasta.cveStatusSubasta)}
                    </span>
                    <span className="text-xs text-gray-400">
                        {subasta.cveStatusSubasta === 1 || subasta.cveStatusSubasta === 2
                            ? `Cierra: ${formatFecha(subasta.fechaFinal)}`
                            : `Finalizó: ${formatFecha(subasta.fechaFinal)}`}
                    </span>
                </div>
            </div>
            <div className="flex flex-col gap-2 flex-shrink-0">
                <button
                    onClick={() => navigate(`/subasta/${subasta.idSubasta}`)}
                    className="bg-gray-100 text-gray-600 text-xs px-4 py-2 rounded-full hover:opacity-75 transition-all"
                >
                    Ver subasta
                </button>
                {finalizada && (
                    <button
                        onClick={() => navigate(`/pago/${subasta.idSubasta}`)}
                        className="bg-yellow-400 text-yellow-900 text-xs px-4 py-2 rounded-full hover:opacity-75 transition-all"
                    >
                        ★ Calificar
                    </button>
                )}
            </div>
        </div>
    )
}

export default DetallesUsuario;