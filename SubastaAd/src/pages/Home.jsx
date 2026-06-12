import { useEffect, useState } from "react";
import SubastaCard from "../components/SubastaCard";

function Home() {
    const [subastas, setSubastas] = useState([]);
    const [cargando, setCargando] = useState(true);

    useEffect(() => {
        fetch("http://localhost:5288/api/subasta/home")
            .then(r => r.json())
            .then(data => setSubastas(data.datos ?? []))
            .catch(console.error)
            .finally(() => setCargando(false));
    }, []);

    return (
        <div className="min-h-screen bg-gray-50">
            <div className="max-w-6xl mx-auto px-4 py-10">
                <div className="flex items-center justify-between mb-6">
                    <h2 className="text-xl font-bold text-gray-800">Subastas activas recientes</h2>
                    <a href="/resultados" className="text-sm text-blue-600 hover:underline font-medium">
                        Ver todas →
                    </a>
                </div>

                {cargando && (
                    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                        {[...Array(8)].map((_, i) => (
                            <div key={i} className="bg-white rounded-2xl h-64 animate-pulse border border-gray-100" />
                        ))}
                    </div>
                )}

                {!cargando && subastas.length === 0 && (
                    <div className="text-center py-20 text-gray-400">
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
            </div>
        </div>
    );
}

export default Home;