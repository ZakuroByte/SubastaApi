import { useState, useEffect, useRef } from 'react'
import { useNavigate } from 'react-router-dom'

function Navbar() {
    const navigate = useNavigate()
    const [busqueda, setBusqueda] = useState('') 
    const token = localStorage.getItem('token')
    const idUsuario = localStorage.getItem('idUsuario')

    const [notificaciones, setNotificaciones] = useState([])
    const [noLeidas, setNoLeidas] = useState(0)
    const [mostrarNotif, setMostrarNotif] = useState(false)
    const [mostrarPerfil, setMostrarPerfil] = useState(false)

    const refNotif = useRef(null)
    const refPerfil = useRef(null)

    //Cargar notificaciones y no leidas
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
            .catch(() => {})

        fetch(`http://localhost:5288/api/Notificacion/usuario/${idUsuario}/noleidas`, {
            headers: { 'Authorization': `Bearer ${token}` }
        })
            .then(res => res.json())
            .then(data => setNoLeidas(data))
            .catch(() => {})
    }, [])

    //Cerrar dropdowns al hacer click fuera
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

    const cerrarSesion = () => {
        localStorage.removeItem('token')
        localStorage.removeItem('idUsuario')
        window.location.href = '/'
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
    
    return (
        <nav className="w-full bg-white shadow-md px-6 py-3 flex items-center gap-4 fixed top-0 left-0 z-10">

            {/* Logo */}
            <div className="flex items-center">
                <button
                    onClick={() => window.location.href = '/home'}
                >
                    <img src="/LogoSubasta.png" alt="LogoSubasta" className="w-30 h-auto" />
                </button>
                
            </div>

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
                    placeholder="Buscar..."
                    className="outline-none text-sm w-full border border-gray-300 rounded-l-full px-4 py-2"
                />
                <button type="submit" className="bg-blue-300 hover:bg-blue-300 px-3 py-2 rounded-r-full border border-blue-300">
                    <img src="/Lupa.png" alt="Lupa" className="w-5 h-5" />
                </button>
            </form>

            {/* Espacio flexible */}
            <div className="flex-1"></div>

            {/* Iconos derecha */}
            {token && (
                <div className="flex items-center gap-4">

                    {/* Crear subasta */}
                    <button
                        onClick={() => window.location.href = '/CrearSubasta'}
                        className="bg-green-600 text-white px-4 py-2 rounded-full hover:opacity-75 transition-all"                    
                    >
                        Crear subasta
                    </button>

                    {/* Notificaciones */}
                    <div className="relative" ref={refNotif}>
                        <button
                            onClick={() => { setMostrarNotif(!mostrarNotif); setMostrarPerfil(false) }}
                            className="relative"
                        >
                            <img src="/campana.webp" alt="Campana" className="w-8 h-8" />
                            {noLeidas > 0 && (
                                <span className="absolute -top-1 -right-1 bg-red-500 text-white text-xs rounded-full w-4 h-4 flex items-center justify-center">
                                    {noLeidas}
                                </span>
                            )}
                        </button>

                        {mostrarNotif && (
                            <div className="absolute right-0 mt-2 w-80 bg-white rounded-xl shadow-xl border border-gray-200 z-20">
                                <div className="flex items-center justify-between px-4 py-3 border-b">
                                    <h3 className="font-semibold text-gray-800">Notificaciones</h3>
                                    {noLeidas > 0 && (
                                        <button
                                            onClick={marcarTodasLeidas}
                                            className="text-xs text-blue-600 hover:underline"
                                        >
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
                                                className={`px-4 py-3 border-b cursor-pointer hover:bg-gray-50 ${!n.leida ? 'bg-blue-50' : ''}`}
                                            >
                                                <p className="text-sm text-gray-700">{n.descripcion}</p>
                                                <p className="text-xs text-gray-400 mt-1">
                                                    {new Date(n.fechaEnvio).toLocaleDateString('es-MX', {
                                                        day: '2-digit', month: 'short', year: 'numeric',
                                                        hour: '2-digit', minute: '2-digit'
                                                    })}
                                                </p>
                                                {!n.leida && (
                                                    <span className="text-xs text-blue-500 font-medium">● No leída</span>
                                                )}
                                            </div>
                                        ))
                                    )}
                                </div>
                            </div>
                        )}
                    </div>

                    
                </div>
            )}
            {/* Perfil */}
            <div className="relative" ref={refPerfil}>
                <button onClick={() => { setMostrarPerfil(!mostrarPerfil); setMostrarNotif(false) }}>
                    <img src="/logo.png" alt="Perfil" className="w-8 h-8 rounded-full" />
                </button>

                {mostrarPerfil && (
                    <div className="absolute right-0 mt-2 w-44 bg-white rounded-xl shadow-xl border border-gray-200 z-20">
                        {token ? (
                            <>
                                <button
                                    onClick={() => window.location.href = '/detalles-usuario'}
                                    className="w-full text-left px-4 py-3 text-sm text-gray-700 hover:bg-gray-50 rounded-t-xl"
                                >
                                    👤 Ver perfil
                                </button>
                                <button
                                    onClick={cerrarSesion}
                                    className="w-full text-left px-4 py-3 text-sm text-red-500 hover:bg-gray-50 rounded-b-xl border-t"
                                >
                                    🚪 Cerrar sesión
                                </button>
                            </>
                        ) : (
                            <button
                                onClick={() => window.location.href = '/'}
                                className="w-full text-left px-4 py-3 text-sm text-gray-700 hover:bg-gray-50 rounded-xl"
                            >
                                    🔑 Iniciar sesión
                            </button>
                        )}
                    </div>
                )}
            </div>    
        </nav>
    )
}

export default Navbar
