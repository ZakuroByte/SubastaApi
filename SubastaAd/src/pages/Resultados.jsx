import { useEffect, useState, useCallback } from "react";
import { useSearchParams } from "react-router-dom";
import SubastaCard from "../components/SubastaCard";

const CATEGORIAS = [
    { id: 1, label: "Vehículos" },
    { id: 2, label: "Inmuebles" },
    { id: 3, label: "General" },
];

const TIPOS = [
    { id: 1, label: "Inglesa" },
    { id: 2, label: "Holandesa" },
    { id: 3, label: "Sellada" },
];

const CONDICIONES = [
    { id: 1, label: "Nuevo" },
    { id: 2, label: "Usado" },
    { id: 3, label: "Reacondicionado" },
];

const ORDEN_OPTS = [
    { value: "recientes", label: "Más recientes" },
    { value: "precio_asc", label: "Menor precio" },
    { value: "precio_desc", label: "Mayor precio" },
    { value: "cierre", label: "Cierra pronto" },
];

function Chip({ label, active, onClick }) {
    return (
        <button
            onClick={onClick}
            className={`px-3 py-1 rounded-full text-xs font-medium border transition-colors ${active
                    ? "bg-blue-600 text-white border-blue-600"
                    : "bg-white text-gray-600 border-gray-200 hover:border-blue-400"
                }`}
        >
            {label}
        </button>
    );
}

function Resultados() {
    const [searchParams, setSearchParams] = useSearchParams();

    const [categoria, setCategoria] = useState(searchParams.get("categoria") ? Number(searchParams.get("categoria")) : null);
    const [tipo, setTipo] = useState(searchParams.get("tipo") ? Number(searchParams.get("tipo")) : null);
    const [condicion, setCondicion] = useState(searchParams.get("condicion") ? Number(searchParams.get("condicion")) : null);
    const [orden, setOrden] = useState(searchParams.get("orden") ?? "recientes");
    const [pagina, setPagina] = useState(1);

    const [resultados, setResultados] = useState([]);
    const [meta, setMeta] = useState(null);
    const [cargando, setCargando] = useState(false);
    const [filtersOpen, setFiltersOpen] = useState(false);

    // Leer el nombre siempre fresco desde la URL (lo escribe la Navbar)
    const nombre = searchParams.get("nombre") ?? "";

    const buscar = useCallback(async (pag = 1) => {
        setCargando(true);

        const params = new URLSearchParams();
        if (nombre) params.set("nombre", nombre);
        if (categoria) params.set("categoria", categoria);
        if (tipo) params.set("tipo", tipo);
        if (condicion) params.set("condicion", condicion);
        params.set("orden", orden);
        params.set("pagina", pag);
        params.set("porPagina", 12);

        setSearchParams(params);

        try {
            const r = await fetch(`http://localhost:5288/api/subasta/buscar?${params}`);
            const data = await r.json();
            setResultados(data.datos ?? []);
            setMeta({ total: data.total, totalPaginas: data.totalPaginas });
            setPagina(pag);
        } catch (err) {
            console.error(err);
        } finally {
            setCargando(false);
        }
    }, [nombre, categoria, tipo, condicion, orden]);

    // Re-buscar cada vez que cambia el nombre en la URL (nueva búsqueda desde Navbar)
    useEffect(() => {
        buscar(1);
    }, [nombre]);

    const limpiarFiltros = () => {
        setCategoria(null);
        setTipo(null);
        setCondicion(null);
        setOrden("recientes");
    };

    const hayFiltros = categoria || tipo || condicion || orden !== "recientes";

    return (
        <div className="min-h-screen bg-gray-50">
            {/* Barra de filtros */}
            <div className="bg-white border-b border-gray-200 sticky top-14 z-10 px-4 py-3">
                <div className="max-w-6xl mx-auto flex items-center gap-3">
                    {/* Título de búsqueda */}
                    <p className="text-sm text-gray-500 flex-1">
                        {nombre
                            ? <>Resultados para <span className="font-semibold text-gray-800">"{nombre}"</span></>
                            : "Todas las subastas activas"
                        }
                        {meta && !cargando &&
                            <span className="ml-2 text-gray-400">({meta.total})</span>
                        }
                    </p>

                    <button
                        onClick={() => setFiltersOpen(v => !v)}
                        className={`border px-4 py-1.5 rounded-full text-sm font-medium transition-colors flex items-center gap-1 ${hayFiltros
                                ? "border-blue-500 text-blue-600 bg-blue-50"
                                : "border-gray-200 text-gray-600 hover:border-gray-400"
                            }`}
                    >
                        <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2a1 1 0 01-.293.707L13 13.414V19a1 1 0 01-.553.894l-4 2A1 1 0 017 21v-7.586L3.293 6.707A1 1 0 013 6V4z" />
                        </svg>
                        Filtros
                        {hayFiltros && <span className="bg-blue-600 text-white text-xs rounded-full w-4 h-4 flex items-center justify-center">!</span>}
                    </button>
                </div>

                {/* Panel de filtros */}
                {filtersOpen && (
                    <div className="max-w-6xl mx-auto mt-3 pt-3 border-t border-gray-100 flex flex-col gap-3">
                        <div className="flex items-center gap-2 flex-wrap">
                            <span className="text-xs font-semibold text-gray-500 w-20">Ordenar</span>
                            {ORDEN_OPTS.map(o => (
                                <Chip key={o.value} label={o.label} active={orden === o.value} onClick={() => setOrden(o.value)} />
                            ))}
                        </div>
                        <div className="flex items-center gap-2 flex-wrap">
                            <span className="text-xs font-semibold text-gray-500 w-20">Categoría</span>
                            {CATEGORIAS.map(c => (
                                <Chip key={c.id} label={c.label} active={categoria === c.id}
                                    onClick={() => setCategoria(v => v === c.id ? null : c.id)} />
                            ))}
                        </div>
                        <div className="flex items-center gap-2 flex-wrap">
                            <span className="text-xs font-semibold text-gray-500 w-20">Tipo</span>
                            {TIPOS.map(t => (
                                <Chip key={t.id} label={t.label} active={tipo === t.id}
                                    onClick={() => setTipo(v => v === t.id ? null : t.id)} />
                            ))}
                        </div>
                        <div className="flex items-center gap-2 flex-wrap">
                            <span className="text-xs font-semibold text-gray-500 w-20">Condición</span>
                            {CONDICIONES.map(c => (
                                <Chip key={c.id} label={c.label} active={condicion === c.id}
                                    onClick={() => setCondicion(v => v === c.id ? null : c.id)} />
                            ))}
                        </div>
                        <div className="flex gap-2 pb-1">
                            <button
                                onClick={() => buscar(1)}
                                className="bg-blue-600 text-white px-5 py-1.5 rounded-full text-sm font-semibold hover:bg-blue-700 transition-colors"
                            >
                                Aplicar
                            </button>
                            {hayFiltros && (
                                <button
                                    onClick={() => { limpiarFiltros(); setTimeout(() => buscar(1), 0); }}
                                    className="border border-gray-200 text-gray-500 px-4 py-1.5 rounded-full text-sm hover:border-gray-400 transition-colors"
                                >
                                    Limpiar
                                </button>
                            )}
                        </div>
                    </div>
                )}
            </div>

            {/* Resultados */}
            <div className="max-w-6xl mx-auto px-4 py-8">
                {cargando && (
                    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                        {[...Array(8)].map((_, i) => (
                            <div key={i} className="bg-white rounded-2xl h-64 animate-pulse border border-gray-100" />
                        ))}
                    </div>
                )}

                {!cargando && resultados.length === 0 && (
                    <div className="text-center py-24 text-gray-400">
                        <p className="text-5xl mb-4">🔍</p>
                        <p className="font-semibold text-gray-600">No encontramos subastas</p>
                        <p className="text-sm mt-1">Intenta con otros términos o ajusta los filtros</p>
                        {hayFiltros && (
                            <button onClick={limpiarFiltros} className="mt-4 text-blue-600 text-sm hover:underline">
                                Quitar filtros
                            </button>
                        )}
                    </div>
                )}

                {!cargando && resultados.length > 0 && (
                    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                        {resultados.map(s => (
                            <SubastaCard key={s.idSubasta} subasta={s} />
                        ))}
                    </div>
                )}

                {/* Paginación */}
                {meta && meta.totalPaginas > 1 && !cargando && (
                    <div className="flex justify-center items-center gap-2 mt-10">
                        <button
                            onClick={() => buscar(pagina - 1)}
                            disabled={pagina === 1}
                            className="px-4 py-2 rounded-full border border-gray-200 text-sm text-gray-600 hover:border-gray-400 disabled:opacity-40 disabled:cursor-not-allowed"
                        >
                            ← Anterior
                        </button>
                        <span className="text-sm text-gray-500">
                            Página {pagina} de {meta.totalPaginas}
                        </span>
                        <button
                            onClick={() => buscar(pagina + 1)}
                            disabled={pagina === meta.totalPaginas}
                            className="px-4 py-2 rounded-full border border-gray-200 text-sm text-gray-600 hover:border-gray-400 disabled:opacity-40 disabled:cursor-not-allowed"
                        >
                            Siguiente →
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
}

export default Resultados;