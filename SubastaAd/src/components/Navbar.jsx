import { useState, useEffect, useRef } from 'react'
import { useNavigate } from 'react-router-dom'

function Navbar() {
    const navigate = useNavigate()
    const [busqueda, setBusqueda] = useState('')
    const token = localStorage.getItem('token')
    const idUsuario = localStorage.getItem('idUsuario')
    const tipoUsuario = localStorage.getItem('tipoUsuario')

    const [notificaciones, setNotificaciones] = useState([])
    const [noLeidas, setNoLeidas] = useState(0)
    const [mostrarNotif, setMostrarNotif] = useState(false)
    const [mostrarPerfil, setMostrarPerfil] = useState(false)

    const refNotif = useRef(null)
    const refPerfil = useRef(null)

    useEffect(() => {
        if (!token || !idUsuario) return

        fetch(`http://localhost:5288/api/Notificacion/usuario/${idUsuario}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        })
            .then(res => res.json())
            .then(data => {
                const ordenadas = data.sort((a, b) => new Date(b.fechaEnvio) - new Date(a.fechaEnvio))
                setNotificaciones(ordenadas)
            })
            .catch(() => { })

        fetch(`http://localhost:5288/api/Notificacion/usuario/${idUsuario}/noleidas`, {
            headers: { 'Authorization': `Bearer ${token}` }
        })
            .then(res => res.json())
            .then(data => setNoLeidas(data))
            .catch(() => { })
    }, [])

    useEffect(() => {
        const handleClick = (e) => {
            if (refNotif.current && !refNotif.current.contains(e.target)) setMostrarNotif(false)
            if (refPerfil.current && !refPerfil.current.contains(e.target)) setMostrarPerfil(false)
        }
        document.addEventListener('mousedown', handleClick)
        return () => document.removeEventListener('mousedown', handleClick)
    }, [])

    const marcarUnaLeida = (idNotificacion) => {
        fetch(`http://localhost:5288/api/Notificacion/${idNotificacion}/leer`, {
            method: 'PUT',
            headers: { 'Authorization': `Bearer ${token}` }
        }).then(() => {
            setNotificaciones(prev => prev.map(n =>
                n.idNotificacion === idNotificacion ? { ...n, leida: true } : n
            ))
            setNoLeidas(prev => Math.max(0, prev - 1))
        })
    }

    const marcarTodasLeidas = () => {
        fetch(`http://localhost:5288/api/Notificacion/usuario/${idUsuario}/leer`, {
            method: 'PUT',
            headers: { 'Authorization': `Bearer ${token}` }
        }).then(() => {
            setNotificaciones(prev => prev.map(n => ({ ...n, leida: true })))
            setNoLeidas(0)
        })
    }

    const cerrarSesion = () => {
        localStorage.removeItem('token')
        localStorage.removeItem('idUsuario')
        localStorage.removeItem('tipoUsuario')
        window.location.href = '/home'
    }

    return (
        <nav className="w-full bg-white border-b border-gray-200 px-6 py-3 flex items-center gap-4 fixed top-0 left-0 z-10">

            {/* Logo */}
            <button onClick={() => navigate('/home')} className="flex items-center gap-2 flex-shrink-0">
                <div className="bg-blue-600 text-white w-8 h-8 rounded-lg flex items-center justify-center font-bold text-sm">B</div>
                <span className="font-bold text-gray-800 text-lg tracking-tight">BidMarket</span>
            </button>

            {/* Barra de búsqueda */}
            <form
                onSubmit={(e) => {
                    e.preventDefault()
                    if (busqueda.trim())
                        navigate(`/resultados?nombre=${encodeURIComponent(busqueda.trim())}`)
                }}
                className="flex items-center w-2/4"
            >
                <input
                    type="text"
                    value={busqueda}
                    onChange={e => setBusqueda(e.target.value)}
                    placeholder="Buscar subastas..."
                    className="outline-none text-sm w-full border border-gray-300 rounded-l-full px-4 py-2 focus:border-blue-400"
                />
                <button type="submit" className="bg-blue-600 hover:bg-blue-700 px-3 py-2 rounded-r-full border border-blue-600 transition-colors">
                    {/* Lupa SVG */}
                    <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                        <path strokeLinecap="round" strokeLinejoin="round" d="M21 21l-4.35-4.35m0 0A7.5 7.5 0 104.5 4.5a7.5 7.5 0 0012.15 12.15z" />
                    </svg>
                </button>
            </form>

            <div className="flex-1" />

            {/* Sin sesión */}
            {!token && (
                <div className="flex items-center gap-2">
                    <button
                        onClick={() => navigate('/login')}
                        className="text-sm text-gray-600 px-4 py-2 rounded-full border border-gray-300 hover:bg-gray-50 transition-all"
                    >
                        Iniciar sesión
                    </button>
                    <button
                        onClick={() => navigate('/signup')}
                        className="text-sm text-white bg-blue-600 px-4 py-2 rounded-full hover:bg-blue-700 transition-all"
                    >
                        Registrarse
                    </button>
                </div>
            )}

            {/* Con sesión */}
            {token && (
                <div className="flex items-center gap-3">

                    {/* Crear subasta */}
                    {tipoUsuario === 'Vendedor' && (
                        <button
                            onClick={() => navigate('/CrearSubasta')}
                            className="flex items-center gap-1.5 bg-green-600 text-white text-sm px-4 py-2 rounded-full hover:bg-green-700 transition-all"
                        >
                            {/* Plus SVG */}
                            <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                                <path strokeLinecap="round" strokeLinejoin="round" d="M12 4v16m8-8H4" />
                            </svg>
                            Crear subasta
                        </button>
                    )}

                    {/* Notificaciones */}
                    <div className="relative" ref={refNotif}>
                        <button
                            onClick={() => { setMostrarNotif(!mostrarNotif); setMostrarPerfil(false) }}
                            className="relative w-9 h-9 flex items-center justify-center rounded-full hover:bg-gray-100 transition-all"
                        >
                            {/* Campana SVG */}
                            <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5 text-gray-600" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.8}>
                                <path strokeLinecap="round" strokeLinejoin="round" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6 6 0 10-12 0v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
                            </svg>
                            {noLeidas > 0 && (
                                <span className="absolute -top-0.5 -right-0.5 bg-red-500 text-white text-xs rounded-full w-4 h-4 flex items-center justify-center leading-none">
                                    {noLeidas}
                                </span>
                            )}
                        </button>

                        {mostrarNotif && (
                            <div className="absolute right-0 mt-2 w-80 bg-white rounded-xl shadow-xl border border-gray-200 z-20">
                                <div className="flex items-center justify-between px-4 py-3 border-b border-gray-100">
                                    <h3 className="font-semibold text-gray-800 text-sm">Notificaciones</h3>
                                    {noLeidas > 0 && (
                                        <button onClick={marcarTodasLeidas} className="text-xs text-blue-600 hover:underline">
                                            Marcar todas como leídas
                                        </button>
                                    )}
                                </div>
                                <div className="max-h-72 overflow-y-auto">
                                    {notificaciones.length === 0 ? (
                                        <p className="text-sm text-gray-400 text-center py-6">No tienes notificaciones</p>
                                    ) : (
                                        notificaciones.map(n => (
                                            <div
                                                key={n.idNotificacion}
                                                onClick={() => !n.leida && marcarUnaLeida(n.idNotificacion)}
                                                className={`px-4 py-3 border-b border-gray-50 cursor-pointer hover:bg-gray-50 ${!n.leida ? 'bg-blue-50' : ''}`}
                                            >
                                                <p className="text-sm text-gray-700">{n.descripcion}</p>
                                                <p className="text-xs text-gray-400 mt-1">
                                                    {new Date(n.fechaEnvio).toLocaleDateString('es-MX', {
                                                        day: '2-digit', month: 'short', year: 'numeric',
                                                        hour: '2-digit', minute: '2-digit'
                                                    })}
                                                </p>
                                                {!n.leida && <span className="text-xs text-blue-500 font-medium">● No leída</span>}
                                            </div>
                                        ))
                                    )}
                                </div>
                            </div>
                        )}
                    </div>

                    {/* Perfil */}
                    <div className="relative" ref={refPerfil}>
                        <button
                            onClick={() => { setMostrarPerfil(!mostrarPerfil); setMostrarNotif(false) }}
                            className="w-9 h-9 flex items-center justify-center rounded-full bg-blue-100 hover:bg-blue-200 transition-all"
                        >
                            {/* Usuario SVG */}
                            <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5 text-blue-700" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.8}>
                                <path strokeLinecap="round" strokeLinejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                            </svg>
                        </button>

                        {mostrarPerfil && (
                            <div className="absolute right-0 mt-2 w-48 bg-white rounded-xl shadow-xl border border-gray-200 z-20 overflow-hidden">
                                <button
                                    onClick={() => { navigate('/detalles-usuario'); setMostrarPerfil(false) }}
                                    className="w-full text-left px-4 py-3 text-sm text-gray-700 hover:bg-gray-50 flex items-center gap-2"
                                >
                                    <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.8}>
                                        <path strokeLinecap="round" strokeLinejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                                    </svg>
                                    Ver perfil
                                </button>
                                <button
                                    onClick={cerrarSesion}
                                    className="w-full text-left px-4 py-3 text-sm text-red-500 hover:bg-gray-50 border-t border-gray-100 flex items-center gap-2"
                                >
                                    <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.8}>
                                        <path strokeLinecap="round" strokeLinejoin="round" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a2 2 0 01-2 2H5a2 2 0 01-2-2V7a2 2 0 012-2h6a2 2 0 012 2v1" />
                                    </svg>
                                    Cerrar sesión
                                </button>
                            </div>
                        )}
                    </div>
                </div>
            )}
        </nav>
    )
}

export default Navbar