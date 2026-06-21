import { useState, useEffect, useRef } from 'react'
import { useParams, useNavigate } from 'react-router-dom'

function DetalleSubasta() {
    const { id } = useParams()
    const navigate = useNavigate()

    const token = localStorage.getItem('token')
    const idUsuario = parseInt(localStorage.getItem('idUsuario'))

    const [subasta, setSubasta] = useState(null)
    const [loading, setLoading] = useState(true)
    const [tiempoRestante, setTiempoRestante] = useState(null)
    const [monto, setMonto] = useState('')
    const [enviando, setEnviando] = useState(false)
    const [error, setError] = useState('')
    const [exito, setExito] = useState('')
    const [fotoActiva, setFotoActiva] = useState(0)

    const intervalRef = useRef(null)

    // Cargar subasta
    useEffect(() => {
        fetch(`http://localhost:5288/api/Subasta/${id}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        })
            .then(res => res.json())
            .then(data => {
                setSubasta(data)
                setLoading(false)
            })
            .catch(() => setLoading(false))
    }, [id])

    // Contador regresivo
    useEffect(() => {
        if (!subasta) return

        const calcular = () => {
            const ahora = new Date()
            const fin = new Date(subasta.fechaFinal)
            const diff = fin - ahora

            if (diff <= 0) {
                setTiempoRestante({ h: '00', m: '00', s: '00' })
                clearInterval(intervalRef.current)
                return
            }

            const h = Math.floor(diff / 3600000)
            const m = Math.floor((diff % 3600000) / 60000)
            const s = Math.floor((diff % 60000) / 1000)

            setTiempoRestante({
                h: String(h).padStart(2, '0'),
                m: String(m).padStart(2, '0'),
                s: String(s).padStart(2, '0'),
            })
        }

        calcular()
        intervalRef.current = setInterval(calcular, 1000)
        return () => clearInterval(intervalRef.current)
    }, [subasta])

    const montoMinimo = subasta
        ? parseFloat(subasta.precioActual) + parseFloat(subasta.incremento ?? 0)
        : 0

    // Oferta inglesa / sellada
    const enviarOferta = async () => {
        setError('')
        setExito('')

        if (!monto || parseFloat(monto) <= 0) {
            setError('Ingresa un monto válido')
            return
        }

        // Verificar si ya tiene oferta en sellada
        if (subasta.cveTipoSubasta === 3) {
            const ofertaExistente = subasta.ofertas?.find(o => o.cveUsuario === idUsuario)
            if (ofertaExistente) {
                // Modificar oferta existente
                setEnviando(true)
                const res = await fetch(`http://localhost:5288/api/Oferta/${ofertaExistente.idOferta}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },
                    body: JSON.stringify({
                        idOferta: ofertaExistente.idOferta,
                        monto: parseFloat(monto),
                        cveUsuario: idUsuario,
                        cveSubasta: subasta.idSubasta
                    })
                })
                setEnviando(false)
                if (res.ok) {
                    setExito('Tu oferta fue actualizada')
                    setMonto('')
                } else {
                    const msg = await res.text()
                    setError(msg || 'Error al actualizar la oferta')
                }
                return
            }
        }

        setEnviando(true)
        const res = await fetch('http://localhost:5288/api/Oferta', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({
                monto: parseFloat(monto),
                cveUsuario: idUsuario,
                cveSubasta: subasta.idSubasta
            })
        })
        setEnviando(false)

        if (res.ok) {
            setExito('¡Oferta enviada correctamente!')
            setMonto('')
            // Recargar subasta para actualizar precio y ofertas
            const updated = await fetch(`http://localhost:5288/api/Subasta/${id}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            }).then(r => r.json())
            setSubasta(updated)
        } else {
            const msg = await res.text()
            setError(msg || 'Error al enviar la oferta')
        }
    }

    // Aceptar precio holandesa
    const aceptarPrecio = async () => {
        setError('')
        setExito('')
        setEnviando(true)

        const res = await fetch('http://localhost:5288/api/Oferta/aceptar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({
                idSubasta: subasta.idSubasta,
                idUsuario: idUsuario
            })
        })
        setEnviando(false)

        if (res.ok) {
            setExito('¡Compra realizada! Revisa tus notificaciones para el pago.')
            const updated = await fetch(`http://localhost:5288/api/Subasta/${id}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            }).then(r => r.json())
            setSubasta(updated)
        } else {
            const msg = await res.text()
            setError(msg || 'Error al aceptar el precio')
        }
    }

    const tipoLabel = (cve) => {
        if (cve === 1) return 'Inglesa'
        if (cve === 2) return 'Holandesa'
        if (cve === 3) return 'Sellada'
        return ''
    }

    const tipoBadgeColor = (cve) => {
        if (cve === 1) return 'bg-blue-100 text-blue-800'
        if (cve === 2) return 'bg-yellow-100 text-yellow-800'
        if (cve === 3) return 'bg-purple-100 text-purple-800'
        return ''
    }

    const statusLabel = (cve) => {
        if (cve === 1) return { label: 'Pendiente', color: 'bg-gray-100 text-gray-700' }
        if (cve === 2) return { label: 'Activa', color: 'bg-green-100 text-green-800' }
        if (cve === 3) return { label: 'Finalizada', color: 'bg-red-100 text-red-700' }
        if (cve === 4) return { label: 'Cancelada', color: 'bg-red-100 text-red-700' }
        return { label: '', color: '' }
    }

    const esVendedor = subasta?.productoRef?.cveUsuario === idUsuario
    const estaActiva = subasta?.cveStatusSubasta === 2
    const tiempoTerminado = tiempoRestante?.h === '00' && tiempoRestante?.m === '00' && tiempoRestante?.s === '00'
    const puedeOfertar = token && !esVendedor && estaActiva && !tiempoTerminado

    if (loading) return (
        <div className="flex justify-center items-center min-h-screen">
            <p className="text-gray-400">Cargando subasta...</p>
        </div>
    )

    if (!subasta) return (
        <div className="flex justify-center items-center min-h-screen">
            <p className="text-gray-400">Subasta no encontrada</p>
        </div>
    )

    const fotos = subasta.productoRef?.fotos ?? []
    const status = statusLabel(subasta.cveStatusSubasta)

    return (
        <div className="max-w-5xl mx-auto px-4 pt-24 pb-10">

            {/* Cabecera */}
            <div className="flex items-center gap-3 mb-4 flex-wrap">
                <span className={`text-xs font-medium px-3 py-1 rounded-full ${status.color}`}>
                    {status.label}
                </span>
                <span className={`text-xs font-medium px-3 py-1 rounded-full ${tipoBadgeColor(subasta.cveTipoSubasta)}`}>
                    Subasta {tipoLabel(subasta.cveTipoSubasta)}
                </span>
                <span className="text-xs text-gray-400 ml-auto">ID #{subasta.idSubasta}</span>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-[1fr_320px] gap-6">

                {/* ── Columna izquierda ── */}
                <div className="flex flex-col gap-4">

                    {/* Fotos */}
                    <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
                        {fotos.length > 0 ? (
                            <img
                                src={`http://localhost:5288${fotos[fotoActiva]?.url}`}
                                alt="Foto del producto"
                                className="w-full h-64 object-cover"
                            />
                        ) : (
                            <div className="w-full h-64 bg-gray-100 flex items-center justify-center text-gray-400 text-sm">
                                Sin fotos
                            </div>
                        )}
                        {fotos.length > 1 && (
                            <div className="flex gap-2 p-3">
                                {fotos.map((f, i) => (
                                    <img
                                        key={i}
                                        src={`http://localhost:5288${f.url}`}
                                        alt=""
                                        onClick={() => setFotoActiva(i)}
                                        className={`w-14 h-14 object-cover rounded-lg cursor-pointer border-2 transition-all ${fotoActiva === i ? 'border-blue-500' : 'border-transparent'}`}
                                    />
                                ))}
                            </div>
                        )}
                    </div>

                    {/* Info del producto */}
                    <div className="bg-white rounded-xl border border-gray-200 p-5">
                        <h1 className="text-xl font-medium text-gray-800 mb-1">
                            {subasta.productoRef?.nombre}
                        </h1>
                        <p className="text-sm text-gray-500 mb-4">
                            {subasta.productoRef?.descripcion}
                        </p>
                        <hr className="border-gray-100 mb-4" />
                        <div className="grid grid-cols-2 gap-3 text-sm">
                            <div>
                                <span className="text-gray-400 text-xs block">Ubicación</span>
                                <span className="text-gray-700">{subasta.productoRef?.ubicacion ?? '—'}</span>
                            </div>
                            <div>
                                <span className="text-gray-400 text-xs block">Inicio</span>
                                <span className="text-gray-700">
                                    {new Date(subasta.fechaInicio).toLocaleDateString('es-MX', {
                                        day: '2-digit', month: 'short', year: 'numeric',
                                        hour: '2-digit', minute: '2-digit'
                                    })}
                                </span>
                            </div>
                            <div>
                                <span className="text-gray-400 text-xs block">Cierre</span>
                                <span className="text-gray-700">
                                    {new Date(subasta.fechaFinal).toLocaleDateString('es-MX', {
                                        day: '2-digit', month: 'short', year: 'numeric',
                                        hour: '2-digit', minute: '2-digit'
                                    })}
                                </span>
                            </div>
                            {subasta.cveTipoSubasta === 1 && subasta.incremento && (
                                <div>
                                    <span className="text-gray-400 text-xs block">Incremento mínimo</span>
                                    <span className="text-gray-700">${Number(subasta.incremento).toLocaleString('es-MX')}</span>
                                </div>
                            )}
                        </div>
                    </div>

                    {/* Historial de ofertas (inglesa) */}
                    {subasta.cveTipoSubasta === 1 && (
                        <div className="bg-white rounded-xl border border-gray-200 p-5">
                            <p className="text-xs font-medium text-gray-400 uppercase tracking-wide mb-3">
                                Historial de ofertas
                            </p>
                            {subasta.ofertas?.length === 0 ? (
                                <p className="text-sm text-gray-400 text-center py-4">Aún no hay ofertas</p>
                            ) : (
                                subasta.ofertas?.map((o, i) => (
                                    <div
                                        key={o.idOferta}
                                        className={`flex justify-between items-center py-2.5 border-b border-gray-100 last:border-none ${i === 0 ? 'bg-green-50 -mx-5 px-5' : ''}`}
                                    >
                                        <div>
                                            <p className="text-sm text-gray-700">
                                                {o.usuarioRef?.nombre ?? `Usuario #${o.cveUsuario}`}
                                                {i === 0 && <span className="ml-2 text-xs text-green-700 font-medium">● Líder</span>}
                                            </p>
                                            <p className="text-xs text-gray-400">
                                                {new Date(o.fecha).toLocaleDateString('es-MX', {
                                                    day: '2-digit', month: 'short',
                                                    hour: '2-digit', minute: '2-digit'
                                                })}
                                            </p>
                                        </div>
                                        <span className={`text-sm font-medium ${i === 0 ? 'text-green-700' : 'text-gray-700'}`}>
                                            ${Number(o.monto).toLocaleString('es-MX')}
                                        </span>
                                    </div>
                                ))
                            )}
                        </div>
                    )}

                    {/* Sellada: aviso confidencial */}
                    {subasta.cveTipoSubasta === 3 && estaActiva && (
                        <div className="bg-purple-50 border border-purple-100 rounded-xl p-5 flex gap-3 items-start">
                            <span className="text-purple-400 text-lg">🔒</span>
                            <p className="text-sm text-purple-700">
                                Esta es una subasta sellada. Las ofertas son confidenciales y se revelarán al finalizar.
                            </p>
                        </div>
                    )}

                    {/* Holandesa: historial visible */}
                    {subasta.cveTipoSubasta === 2 && subasta.ofertas?.length > 0 && (
                        <div className="bg-white rounded-xl border border-gray-200 p-5">
                            <p className="text-xs font-medium text-gray-400 uppercase tracking-wide mb-3">
                                Registro de compra
                            </p>
                            {subasta.ofertas.map(o => (
                                <div key={o.idOferta} className="flex justify-between items-center py-2">
                                    <p className="text-sm text-gray-700">
                                        {o.usuarioRef?.nombre ?? `Usuario #${o.cveUsuario}`}
                                    </p>
                                    <span className="text-sm font-medium text-yellow-700">
                                        ${Number(o.monto).toLocaleString('es-MX')}
                                    </span>
                                </div>
                            ))}
                        </div>
                    )}
                </div>

                {/* ── Columna derecha ── */}
                <div className="flex flex-col gap-4">

                    {/* Contador */}
                    <div className="bg-white rounded-xl border border-gray-200 p-5">
                        <div className="bg-gray-50 rounded-lg p-4 text-center mb-4">
                            <p className="text-xs text-gray-400 mb-2">⏱ Tiempo restante</p>
                            {tiempoRestante ? (
                                <div className="flex justify-center items-baseline gap-1">
                                    {[
                                        { val: tiempoRestante.h, label: 'h' },
                                        { val: tiempoRestante.m, label: 'm' },
                                        { val: tiempoRestante.s, label: 's' },
                                    ].map((t, i) => (
                                        <span key={i} className="flex items-baseline gap-0.5">
                                            <span className="text-3xl font-medium tabular-nums text-gray-800">{t.val}</span>
                                            <span className="text-xs text-gray-400 mr-1">{t.label}</span>
                                            {i < 2 && <span className="text-2xl text-gray-300 mr-1">:</span>}
                                        </span>
                                    ))}
                                </div>
                            ) : (
                                <p className="text-sm text-gray-400">Calculando...</p>
                            )}
                        </div>

                        {/* Precio */}
                        <div className="flex justify-between items-baseline mb-1">
                            <span className="text-sm text-gray-400">
                                {subasta.cveTipoSubasta === 2 ? 'Precio actual (baja cada 10 min)' : 'Precio actual'}
                            </span>
                            <span className="text-2xl font-medium text-gray-800">
                                ${Number(subasta.precioActual).toLocaleString('es-MX')}
                            </span>
                        </div>
                        <div className="flex justify-between items-baseline">
                            <span className="text-sm text-gray-400">Precio inicial</span>
                            <span className="text-sm text-gray-600">
                                ${Number(subasta.precioInicial).toLocaleString('es-MX')}
                            </span>
                        </div>
                    </div>

                    {/* Mensajes de error / éxito */}
                    {error && (
                        <div className="bg-red-50 border border-red-100 text-red-600 text-sm px-4 py-3 rounded-xl">
                            {error}
                        </div>
                    )}
                    {exito && (
                        <div className="bg-green-50 border border-green-100 text-green-700 text-sm px-4 py-3 rounded-xl">
                            {exito}
                        </div>
                    )}

                    {/* Formulario según tipo */}
                    {!token && (
                        <div className="bg-white rounded-xl border border-gray-200 p-5 text-center">
                            <p className="text-sm text-gray-500 mb-3">Inicia sesión para participar</p>
                            <button
                                onClick={() => navigate('/')}
                                className="bg-blue-600 text-white px-6 py-2 rounded-full text-sm hover:opacity-75 transition-all"
                            >
                                Iniciar sesión
                            </button>
                        </div>
                    )}

                    {esVendedor && (
                        <div className="bg-gray-50 border border-gray-200 rounded-xl p-4 text-sm text-gray-500 text-center">
                            Eres el vendedor de esta subasta
                        </div>
                    )}

                    {/* Inglesa o Sellada */}
                    {puedeOfertar && (subasta.cveTipoSubasta === 1 || subasta.cveTipoSubasta === 3) && (
                        <div className="bg-white rounded-xl border border-gray-200 p-5">
                            <p className="text-xs font-medium text-gray-400 uppercase tracking-wide mb-3">
                                {subasta.cveTipoSubasta === 3 ? 'Tu oferta (confidencial)' : 'Enviar oferta'}
                            </p>
                            <label className="text-sm text-gray-500 block mb-1.5">Monto (MXN)</label>
                            <input
                                type="number"
                                value={monto}
                                onChange={e => { setMonto(e.target.value); setError(''); setExito('') }}
                                placeholder={subasta.cveTipoSubasta === 1 ? `Mín. $${montoMinimo.toLocaleString('es-MX')}` : 'Tu monto'}
                                className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm outline-none focus:border-blue-400"
                            />
                            {subasta.cveTipoSubasta === 1 && (
                                <p className="text-xs text-gray-400 mt-1">
                                    Debe superar ${montoMinimo.toLocaleString('es-MX')} (precio actual + incremento)
                                </p>
                            )}
                            {subasta.cveTipoSubasta === 3 && (
                                <p className="text-xs text-gray-400 mt-1">
                                    {subasta.ofertas?.find(o => o.cveUsuario === idUsuario)
                                        ? 'Ya tienes una oferta. Puedes modificarla antes del cierre.'
                                        : 'Solo puedes enviar una oferta. Podrás modificarla antes del cierre.'}
                                </p>
                            )}
                            <button
                                onClick={enviarOferta}
                                disabled={enviando}
                                className={`w-full mt-3 text-white py-2.5 rounded-full text-sm font-medium hover:opacity-75 transition-all disabled:opacity-50 ${subasta.cveTipoSubasta === 3 ? 'bg-purple-600' : 'bg-blue-600'}`}
                            >
                                {enviando ? 'Enviando...' : (
                                    subasta.cveTipoSubasta === 3
                                        ? (subasta.ofertas?.find(o => o.cveUsuario === idUsuario) ? 'Actualizar oferta' : 'Enviar oferta')
                                        : 'Enviar oferta'
                                )}
                            </button>
                        </div>
                    )}

                    {/* Holandesa */}
                    {puedeOfertar && subasta.cveTipoSubasta === 2 && (
                        <div className="bg-white rounded-xl border border-gray-200 p-5">
                            <p className="text-xs font-medium text-gray-400 uppercase tracking-wide mb-3">
                                Aceptar precio
                            </p>
                            <div className="flex justify-between items-baseline mb-3">
                                <span className="text-sm text-gray-400">Precio actual</span>
                                <span className="text-2xl font-medium text-yellow-700">
                                    ${Number(subasta.precioActual).toLocaleString('es-MX')}
                                </span>
                            </div>
                            <p className="text-xs text-gray-400 mb-3">
                                El precio disminuye automáticamente con el tiempo. Acepta ahora para comprar al precio actual.
                            </p>
                            <button
                                onClick={aceptarPrecio}
                                disabled={enviando}
                                className="w-full bg-green-600 text-white py-2.5 rounded-full text-sm font-medium hover:opacity-75 transition-all disabled:opacity-50"
                            >
                                {enviando ? 'Procesando...' : '✓ Aceptar precio actual'}
                            </button>
                        </div>
                    )}

                    {/* Subasta finalizada */}
                    {!estaActiva && subasta.cveStatusSubasta === 3 && (
                        <div className="bg-gray-50 border border-gray-100 rounded-xl p-5 text-center">
                            <p className="text-sm font-medium text-gray-600 mb-1">Subasta finalizada</p>
                            {subasta.usuarioGanadorRef && (
                                <p className="text-xs text-gray-400">
                                    Ganador: {subasta.usuarioGanadorRef.nombre} {subasta.usuarioGanadorRef.apellidoPaterno}
                                </p>
                            )}
                        </div>
                    )}
                    {subasta.cveStatusSubasta === 3 && subasta.cveUsuarioGanador === idUsuario && (
                        <button onClick={() => navigate(`/pago/${subasta.idSubasta}`)}
                            className="w-full bg-blue-600 text-white py-2.5 rounded-full text-sm font-medium hover:opacity-75 transition-all">
                            💳 Ir a pagar
                        </button>
                    )}
                </div>
            </div>
        </div>
    )
}

export default DetalleSubasta