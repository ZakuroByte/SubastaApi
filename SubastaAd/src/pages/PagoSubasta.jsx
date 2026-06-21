import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'

function PagoSubasta() {
    const { id } = useParams() // idSubasta
    const navigate = useNavigate()

    const token = localStorage.getItem('token')
    const idUsuario = parseInt(localStorage.getItem('idUsuario'))

    const [pago, setPago] = useState(null)
    const [subasta, setSubasta] = useState(null)
    const [calificacion, setCalificacion] = useState(null)
    const [loading, setLoading] = useState(true)

    // Pasarela simulada
    const [paso, setPaso] = useState('resumen') // resumen | formulario | procesando | exito
    const [tarjeta, setTarjeta] = useState({ numero: '', nombre: '', expiracion: '', cvv: '' })

    // Calificación
    const [estrellas, setEstrellas] = useState(0)
    const [comentario, setComentario] = useState('')
    const [enviandoCal, setEnviandoCal] = useState(false)
    const [exitoCal, setExitoCal] = useState('')
    const [errorCal, setErrorCal] = useState('')

    const [error, setError] = useState('')

    useEffect(() => {
        if (!token) { navigate('/'); return }

        Promise.all([
            fetch(`http://localhost:5288/api/Pago/subasta/${id}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            }).then(r => r.ok ? r.json() : null),

            fetch(`http://localhost:5288/api/Subasta/${id}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            }).then(r => r.ok ? r.json() : null),
        ]).then(([pagoData, subastaData]) => {
            setPago(pagoData)
            setSubasta(subastaData)

            // Verificar si ya calificó
            if (subastaData) {
                fetch(`http://localhost:5288/api/Calificacion/usuario/${idUsuario}`, {
                    headers: { 'Authorization': `Bearer ${token}` }
                })
                    .then(r => r.json())
                    .then(cals => {
                        const cal = cals.find(c => c.cveSubasta === subastaData.idSubasta && c.cveUsuarioCalificador === idUsuario)
                        setCalificacion(cal ?? null)
                    })
                    .catch(() => {})
            }

            setLoading(false)
        }).catch(() => setLoading(false))
    }, [id])

    const formatFecha = (fecha) => new Date(fecha).toLocaleDateString('es-MX', {
        day: '2-digit', month: 'short', year: 'numeric',
        hour: '2-digit', minute: '2-digit'
    })

    const formatMonto = (m) => `$${Number(m).toLocaleString('es-MX')}`

    const tiempoVencido = pago ? new Date() > new Date(pago.fechaLimite) : false

    // Determinar rol del usuario
    const esComprador = subasta?.cveUsuarioGanador === idUsuario
    const esVendedor = subasta?.productoRef?.cveUsuario === idUsuario

    // A quién calificar
    const idACalificar = esComprador
        ? subasta?.productoRef?.cveUsuario   // comprador califica al vendedor
        : subasta?.cveUsuarioGanador          // vendedor califica al comprador

    const formatTarjeta = (val) => val.replace(/\D/g, '').slice(0, 16).replace(/(.{4})/g, '$1 ').trim()
    const formatExp = (val) => {
        const v = val.replace(/\D/g, '').slice(0, 4)
        return v.length >= 3 ? `${v.slice(0, 2)}/${v.slice(2)}` : v
    }

    const procesarPago = async () => {
        setError('')
        const { numero, nombre, expiracion, cvv } = tarjeta

        if (numero.replace(/\s/g, '').length < 16) { setError('Número de tarjeta inválido'); return }
        if (!nombre.trim()) { setError('Ingresa el nombre del titular'); return }
        if (expiracion.length < 5) { setError('Fecha de expiración inválida'); return }
        if (cvv.length < 3) { setError('CVV inválido'); return }

        setPaso('procesando')

        // Simular procesamiento
        await new Promise(r => setTimeout(r, 2200))

        const res = await fetch(`http://localhost:5288/api/Pago/${pago.idPago}/pagar`, {
            method: 'PUT',
            headers: { 'Authorization': `Bearer ${token}` }
        })

        if (res.ok) {
            setPago(prev => ({ ...prev, cveStatusPago: 2, fechaRealizacion: new Date().toISOString() }))
            setPaso('exito')
        } else {
            const msg = await res.text()
            setError(msg || 'Error al procesar el pago')
            setPaso('formulario')
        }
    }

    const enviarCalificacion = async () => {
        setErrorCal('')
        setExitoCal('')

        if (estrellas === 0) { setErrorCal('Selecciona una calificación de 1 a 5 estrellas'); return }

        setEnviandoCal(true)

        const res = await fetch('http://localhost:5288/api/Calificacion', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({
                estrellas,
                comentario,
                cveUsuarioCalificado: idACalificar,
                cveUsuarioCalificador: idUsuario,
                cveSubasta: subasta.idSubasta
            })
        })

        setEnviandoCal(false)

        if (res.ok) {
            setExitoCal('¡Calificación enviada!')
            setCalificacion({ estrellas, comentario })
        } else {
            const msg = await res.text()
            setErrorCal(msg || 'Error al enviar la calificación')
        }
    }

    if (loading) return (
        <div className="flex justify-center items-center min-h-screen">
            <p className="text-gray-400">Cargando...</p>
        </div>
    )

    if (!pago || !subasta) return (
        <div className="flex justify-center items-center min-h-screen">
            <p className="text-gray-400">No se encontró la información del pago</p>
        </div>
    )

    const pagado = pago.cveStatusPago === 2

    return (
        <div className="max-w-2xl mx-auto px-4 pt-24 pb-12">

            {/* Header */}
            <button onClick={() => navigate(`/subasta/${id}`)} className="text-sm text-blue-600 hover:underline mb-6 block">
                ← Volver a la subasta
            </button>

            <h1 className="text-2xl font-semibold text-gray-800 mb-6">
                {pagado ? '✅ Pago completado' : '💳 Realizar pago'}
            </h1>

            {/* Resumen de la subasta */}
            <div className="bg-white rounded-2xl border border-gray-200 p-5 mb-4">
                <p className="text-xs font-medium text-gray-400 uppercase tracking-wide mb-3">Resumen</p>
                <div className="flex gap-4 items-center">
                    {subasta.productoRef?.fotos?.[0]?.url ? (
                        <img
                            src={`http://localhost:5288${subasta.productoRef.fotos[0].url}`}
                            alt=""
                            className="w-20 h-20 object-cover rounded-xl border border-gray-100"
                        />
                    ) : (
                        <div className="w-20 h-20 bg-gray-100 rounded-xl flex items-center justify-center text-gray-300 text-2xl">📦</div>
                    )}
                    <div className="flex-1">
                        <p className="font-medium text-gray-800">{subasta.productoRef?.nombre}</p>
                        <p className="text-xs text-gray-400 mt-0.5">{subasta.productoRef?.ubicacion}</p>
                        <p className="text-xl font-bold text-gray-900 mt-2">{formatMonto(pago.monto)}</p>
                    </div>
                </div>
                <hr className="border-gray-100 my-4" />
                <div className="grid grid-cols-2 gap-3 text-sm">
                    <div>
                        <span className="text-gray-400 text-xs block">Estado del pago</span>
                        <span className={`font-medium ${pagado ? 'text-green-600' : tiempoVencido ? 'text-red-500' : 'text-yellow-600'}`}>
                            {pagado ? 'Pagado' : tiempoVencido ? 'Vencido' : 'Pendiente'}
                        </span>
                    </div>
                    <div>
                        <span className="text-gray-400 text-xs block">Fecha límite</span>
                        <span className={`font-medium ${tiempoVencido && !pagado ? 'text-red-500' : 'text-gray-700'}`}>
                            {formatFecha(pago.fechaLimite)}
                        </span>
                    </div>
                    {pagado && pago.fechaRealizacion && (
                        <div>
                            <span className="text-gray-400 text-xs block">Pagado el</span>
                            <span className="text-gray-700">{formatFecha(pago.fechaRealizacion)}</span>
                        </div>
                    )}
                    <div>
                        <span className="text-gray-400 text-xs block">Tu rol</span>
                        <span className="text-gray-700">{esComprador ? 'Comprador' : 'Vendedor'}</span>
                    </div>
                </div>
            </div>

            {/* Pasarela simulada — solo para el comprador y si no está pagado */}
            {esComprador && !pagado && !tiempoVencido && (
                <div className="bg-white rounded-2xl border border-gray-200 p-5 mb-4">
                    <p className="text-xs font-medium text-gray-400 uppercase tracking-wide mb-4">Datos de pago</p>

                    {paso === 'resumen' && (
                        <button
                            onClick={() => setPaso('formulario')}
                            className="w-full bg-blue-600 text-white py-3 rounded-full font-medium hover:opacity-80 transition-all"
                        >
                            Proceder al pago — {formatMonto(pago.monto)}
                        </button>
                    )}

                    {(paso === 'formulario' || paso === 'procesando') && (
                        <div className="flex flex-col gap-3">
                            {/* Número de tarjeta */}
                            <div>
                                <label className="text-sm text-gray-500 block mb-1">Número de tarjeta</label>
                                <input
                                    type="text"
                                    inputMode="numeric"
                                    placeholder="1234 5678 9012 3456"
                                    value={tarjeta.numero}
                                    onChange={e => setTarjeta(p => ({ ...p, numero: formatTarjeta(e.target.value) }))}
                                    className="w-full border border-gray-300 rounded-lg px-3 py-2.5 text-sm outline-none focus:border-blue-400 tracking-widest"
                                    maxLength={19}
                                    disabled={paso === 'procesando'}
                                />
                            </div>
                            {/* Nombre */}
                            <div>
                                <label className="text-sm text-gray-500 block mb-1">Nombre del titular</label>
                                <input
                                    type="text"
                                    placeholder="Como aparece en la tarjeta"
                                    value={tarjeta.nombre}
                                    onChange={e => setTarjeta(p => ({ ...p, nombre: e.target.value.toUpperCase() }))}
                                    className="w-full border border-gray-300 rounded-lg px-3 py-2.5 text-sm outline-none focus:border-blue-400 uppercase"
                                    disabled={paso === 'procesando'}
                                />
                            </div>
                            {/* Expiración y CVV */}
                            <div className="grid grid-cols-2 gap-3">
                                <div>
                                    <label className="text-sm text-gray-500 block mb-1">Expiración</label>
                                    <input
                                        type="text"
                                        inputMode="numeric"
                                        placeholder="MM/AA"
                                        value={tarjeta.expiracion}
                                        onChange={e => setTarjeta(p => ({ ...p, expiracion: formatExp(e.target.value) }))}
                                        className="w-full border border-gray-300 rounded-lg px-3 py-2.5 text-sm outline-none focus:border-blue-400"
                                        maxLength={5}
                                        disabled={paso === 'procesando'}
                                    />
                                </div>
                                <div>
                                    <label className="text-sm text-gray-500 block mb-1">CVV</label>
                                    <input
                                        type="password"
                                        inputMode="numeric"
                                        placeholder="•••"
                                        value={tarjeta.cvv}
                                        onChange={e => setTarjeta(p => ({ ...p, cvv: e.target.value.replace(/\D/g, '').slice(0, 4) }))}
                                        className="w-full border border-gray-300 rounded-lg px-3 py-2.5 text-sm outline-none focus:border-blue-400"
                                        maxLength={4}
                                        disabled={paso === 'procesando'}
                                    />
                                </div>
                            </div>

                            {error && (
                                <p className="text-sm text-red-500 bg-red-50 px-3 py-2 rounded-lg">{error}</p>
                            )}

                            <button
                                onClick={procesarPago}
                                disabled={paso === 'procesando'}
                                className="w-full bg-blue-600 text-white py-3 rounded-full font-medium hover:opacity-80 transition-all disabled:opacity-60 mt-1 flex items-center justify-center gap-2"
                            >
                                {paso === 'procesando' ? (
                                    <>
                                        <svg className="animate-spin w-4 h-4" viewBox="0 0 24 24" fill="none">
                                            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"/>
                                            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8z"/>
                                        </svg>
                                        Procesando pago...
                                    </>
                                ) : `Pagar ${formatMonto(pago.monto)}`}
                            </button>

                            <p className="text-xs text-gray-400 text-center">🔒 Pago simulado — ningún dato es real</p>
                        </div>
                    )}

                    {paso === 'exito' && (
                        <div className="text-center py-4">
                            <div className="text-5xl mb-3">✅</div>
                            <p className="font-medium text-green-700 text-lg">¡Pago realizado!</p>
                            <p className="text-sm text-gray-400 mt-1">El vendedor ha sido notificado</p>
                        </div>
                    )}
                </div>
            )}

            {/* Aviso vendedor esperando pago */}
            {esVendedor && !pagado && (
                <div className={`rounded-2xl border p-5 mb-4 ${tiempoVencido ? 'bg-red-50 border-red-100' : 'bg-yellow-50 border-yellow-100'}`}>
                    <p className={`text-sm font-medium ${tiempoVencido ? 'text-red-700' : 'text-yellow-700'}`}>
                        {tiempoVencido
                            ? '⚠️ El tiempo límite de pago venció. El comprador no realizó el pago.'
                            : '⏳ Esperando que el comprador realice el pago...'}
                    </p>
                    {!tiempoVencido && (
                        <p className="text-xs text-yellow-600 mt-1">
                            Fecha límite: {formatFecha(pago.fechaLimite)}
                        </p>
                    )}
                </div>
            )}

            {/* Sección de calificación — solo si el pago está hecho */}
            {pagado && (esComprador || esVendedor) && (
                <div className="bg-white rounded-2xl border border-gray-200 p-5">
                    <p className="text-xs font-medium text-gray-400 uppercase tracking-wide mb-3">
                        {esComprador ? 'Califica al vendedor' : 'Califica al comprador'}
                    </p>

                    {calificacion ? (
                        <div className="text-center py-4">
                            <div className="flex justify-center gap-1 mb-2">
                                {[1, 2, 3, 4, 5].map(i => (
                                    <span key={i} className={`text-2xl ${i <= calificacion.estrellas ? 'text-yellow-400' : 'text-gray-200'}`}>★</span>
                                ))}
                            </div>
                            <p className="text-sm text-gray-500">Ya enviaste tu calificación</p>
                            {calificacion.comentario && (
                                <p className="text-sm text-gray-600 mt-2 italic">"{calificacion.comentario}"</p>
                            )}
                        </div>
                    ) : (
                        <div className="flex flex-col gap-3">
                            {/* Estrellas */}
                            <div>
                                <label className="text-sm text-gray-500 block mb-2">Calificación</label>
                                <div className="flex gap-2">
                                    {[1, 2, 3, 4, 5].map(i => (
                                        <button
                                            key={i}
                                            onClick={() => setEstrellas(i)}
                                            className={`text-3xl transition-transform hover:scale-110 ${i <= estrellas ? 'text-yellow-400' : 'text-gray-200'}`}
                                        >
                                            ★
                                        </button>
                                    ))}
                                </div>
                            </div>

                            {/* Comentario */}
                            <div>
                                <label className="text-sm text-gray-500 block mb-1">Comentario (opcional)</label>
                                <textarea
                                    rows={3}
                                    placeholder="Describe tu experiencia..."
                                    value={comentario}
                                    onChange={e => setComentario(e.target.value)}
                                    className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm outline-none focus:border-blue-400 resize-none"
                                />
                            </div>

                            {errorCal && (
                                <p className="text-sm text-red-500 bg-red-50 px-3 py-2 rounded-lg">{errorCal}</p>
                            )}
                            {exitoCal && (
                                <p className="text-sm text-green-600 bg-green-50 px-3 py-2 rounded-lg">{exitoCal}</p>
                            )}

                            <button
                                onClick={enviarCalificacion}
                                disabled={enviandoCal || estrellas === 0}
                                className="w-full bg-yellow-400 text-yellow-900 font-medium py-2.5 rounded-full hover:opacity-80 transition-all disabled:opacity-50"
                            >
                                {enviandoCal ? 'Enviando...' : 'Enviar calificación ★'}
                            </button>
                        </div>
                    )}
                </div>
            )}
        </div>
    )
}

export default PagoSubasta