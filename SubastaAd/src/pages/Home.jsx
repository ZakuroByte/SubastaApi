import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import SubastaCard from "../components/SubastaCard";

const CATEGORIAS = [
    { id: 1, label: 'Vehículos', icon: '🚗' },
    { id: 2, label: 'Inmuebles', icon: '🏠' },
    { id: 3, label: 'Electrónicos', icon: '💻' },
    { id: 4, label: 'Arte', icon: '🎨' },
    { id: 5, label: 'Joyería', icon: '💎' },
    { id: 6, label: 'Otros', icon: '📦' },
]

const TIPOS = [
    {
        tipo: 'Inglesa',
        color: 'bg-blue-50 border-blue-100',
        iconColor: 'text-blue-500',
        icon: (
            <svg xmlns="http://www.w3.org/2000/svg" className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.8}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
            </svg>
        ),
        desc: 'El precio sube con cada oferta. Gana quien más oferte.'
    },
    {
        tipo: 'Holandesa',
        color: 'bg-orange-50 border-orange-100',
        iconColor: 'text-orange-500',
        icon: (
            <svg xmlns="http://www.w3.org/2000/svg" className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.8}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M13 17h8m0 0V9m0 8l-8-8-4 4-6-6" />
            </svg>
        ),
        desc: 'El precio baja con el tiempo. Acepta antes de que alguien más lo haga.'
    },
    {
        tipo: 'Sellada',
        color: 'bg-purple-50 border-purple-100',
        iconColor: 'text-purple-500',
        icon: (
            <svg xmlns="http://www.w3.org/2000/svg" className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.8}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
            </svg>
        ),
        desc: 'Todos ofertan en secreto. Al final gana la oferta más alta.'
    },
]

function Home() {
    const navigate = useNavigate()
    const [subastas, setSubastas] = useState([]);
    const [cargando, setCargando] = useState(true);
    const token = localStorage.getItem('token')

    useEffect(() => {
        fetch("http://localhost:5288/api/subasta/home")
            .then(r => r.json())
            .then(data => setSubastas(data.datos ?? []))
            .catch(console.error)
            .finally(() => setCargando(false));
    }, []);

    return (
        <div className="min-h-screen bg-gray-50">

            {/* ── Hero ── */}
            <div className="bg-gradient-to-br from-blue-700 to-blue-500 pt-24 pb-16 px-4">
                <div className="max-w-3xl mx-auto text-center">
                    <span className="inline-block bg-white/20 text-white text-xs font-medium px-3 py-1 rounded-full mb-4 backdrop-blur-sm">
                        🔨 Plataforma de subastas en línea
                    </span>
                    <h1 className="text-4xl sm:text-5xl font-bold text-white leading-tight mb-4">
                        Compra y vende en<br />
                        <span className="text-blue-200">tiempo real</span>
                    </h1>
                    <p className="text-blue-100 text-lg mb-8 max-w-xl mx-auto">
                        Participa en subastas de vehículos, inmuebles, arte y más. Ofertas en vivo, resultados al instante.
                    </p>
                    <div className="flex gap-3 justify-center flex-wrap">
                        <button
                            onClick={() => navigate('/resultados')}
                            className="bg-white text-blue-700 font-semibold px-6 py-3 rounded-full hover:bg-blue-50 transition-all shadow-md"
                        >
                            Ver todas las subastas
                        </button>
                        {!token && (
                            <button
                                onClick={() => navigate('/signup')}
                                className="bg-blue-800/50 text-white font-medium px-6 py-3 rounded-full hover:bg-blue-800/70 transition-all border border-white/20"
                            >
                                Crear cuenta gratis
                            </button>
                        )}
                    </div>
                </div>
            </div>

            {/* ── Stats rápidos ── */}
            <div className="max-w-4xl mx-auto px-4 -mt-6">
                <div className="bg-white rounded-2xl border border-gray-200 shadow-sm grid grid-cols-3 divide-x divide-gray-100">
                    {[
                        { label: 'Subastas activas', valor: subastas.filter(s => s.cveStatusSubasta === 2).length },
                        { label: 'Categorías', valor: CATEGORIAS.length },
                        { label: 'Tipos de subasta', valor: 3 },
                    ].map((s, i) => (
                        <div key={i} className="py-5 text-center">
                            <p className="text-2xl font-bold text-blue-600">{s.valor}</p>
                            <p className="text-xs text-gray-400 mt-0.5">{s.label}</p>
                        </div>
                    ))}
                </div>
            </div>

            <div className="max-w-6xl mx-auto px-4 py-12 flex flex-col gap-14">

                {/* ── Categorías ── */}
                <section>
                    <h2 className="text-lg font-bold text-gray-800 mb-4">Explorar por categoría</h2>
                    <div className="grid grid-cols-3 sm:grid-cols-6 gap-3">
                        {CATEGORIAS.map(c => (
                            <button
                                key={c.id}
                                onClick={() => navigate(`/resultados?categoria=${c.id}`)}
                                className="bg-white border border-gray-200 rounded-2xl py-4 flex flex-col items-center gap-2 hover:border-blue-300 hover:shadow-sm transition-all"
                            >
                                <span className="text-2xl">{c.icon}</span>
                                <span className="text-xs font-medium text-gray-600">{c.label}</span>
                            </button>
                        ))}
                    </div>
                </section>

                {/* ── Tipos de subasta ── */}
                <section>
                    <h2 className="text-lg font-bold text-gray-800 mb-4">¿Cómo funcionan las subastas?</h2>
                    <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                        {TIPOS.map(t => (
                            <div key={t.tipo} className={`rounded-2xl border p-5 ${t.color}`}>
                                <div className={`mb-3 ${t.iconColor}`}>{t.icon}</div>
                                <p className="font-semibold text-gray-800 mb-1">Subasta {t.tipo}</p>
                                <p className="text-sm text-gray-500">{t.desc}</p>
                            </div>
                        ))}
                    </div>
                </section>

                {/* ── Subastas recientes ── */}
                <section>
                    <div className="flex items-center justify-between mb-4">
                        <h2 className="text-lg font-bold text-gray-800">Subastas activas recientes</h2>
                        <button
                            onClick={() => navigate('/resultados')}
                            className="text-sm text-blue-600 hover:underline font-medium"
                        >
                            Ver todas →
                        </button>
                    </div>

                    {cargando && (
                        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                            {[...Array(8)].map((_, i) => (
                                <div key={i} className="bg-white rounded-2xl h-64 animate-pulse border border-gray-100" />
                            ))}
                        </div>
                    )}

                    {!cargando && subastas.length === 0 && (
                        <div className="text-center py-20 text-gray-400 bg-white rounded-2xl border border-gray-100">
                            <p className="text-4xl mb-3">🔨</p>
                            <p className="font-medium">No hay subastas activas por ahora</p>
                            <p className="text-sm mt-1">Vuelve pronto o crea una</p>
                        </div>
                    )}

                    {!cargando && subastas.length > 0 && (
                        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                            {subastas.map(s => (
                                <SubastaCard key={s.idSubasta} subasta={s} />
                            ))}
                        </div>
                    )}
                </section>

                {/* ── CTA final (solo sin sesión) ── */}
                {!token && (
                    <section className="bg-blue-600 rounded-2xl p-8 text-center">
                        <h3 className="text-2xl font-bold text-white mb-2">¿Listo para participar?</h3>
                        <p className="text-blue-100 mb-6 text-sm">Crea tu cuenta gratis y empieza a ofertar en segundos</p>
                        <div className="flex gap-3 justify-center">
                            <button
                                onClick={() => navigate('/signup')}
                                className="bg-white text-blue-700 font-semibold px-6 py-2.5 rounded-full hover:bg-blue-50 transition-all"
                            >
                                Crear cuenta
                            </button>
                            <button
                                onClick={() => navigate('/')}
                                className="text-white border border-white/30 px-6 py-2.5 rounded-full hover:bg-white/10 transition-all text-sm"
                            >
                                Iniciar sesión
                            </button>
                        </div>
                    </section>
                )}
            </div>
        </div>
    );
}

export default Home;