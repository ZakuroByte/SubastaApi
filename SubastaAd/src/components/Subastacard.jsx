function SubastaCard({ subasta }) {
    console.log(subasta) // ← agrega esto
    const foto = subasta.productoRef?.fotos?.[0]?.url;
    const nombre = subasta.productoRef?.nombre ?? "Sin nombre";
    const tipo = subasta.tipoSubastaRef?.descripcion ?? "";

    const fechaFinal = new Date(subasta.fechaFinal);
    const ahora = new Date();
    const diff = fechaFinal - ahora;

    const tiempoRestante = () => {
        if (diff <= 0) return "Finalizada";
        const h = Math.floor(diff / 1000 / 60 / 60);
        const m = Math.floor((diff / 1000 / 60) % 60);
        if (h > 48) return `${Math.floor(h / 24)} días`;
        if (h > 0) return `${h}h ${m}m`;
        return `${m}m`;
    };

    const badgeColor = {
        "Inglesa": "bg-blue-100 text-blue-700",
        "Holandesa": "bg-orange-100 text-orange-700",
        "Sellada": "bg-purple-100 text-purple-700",
    }[tipo] ?? "bg-gray-100 text-gray-600";

    return (
        <a
            href={`/subasta/${subasta.idSubasta}`}
            className="group bg-white rounded-2xl overflow-hidden shadow-sm hover:shadow-md transition-shadow border border-gray-100 flex flex-col"
        >
            {/* Imagen */}
            <div className="relative h-44 bg-gray-100 overflow-hidden">
                {foto
                    ? <img
                        src={`http://localhost:5288${foto}`}
                        alt={nombre}
                        className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                    />
                    : <div className="w-full h-full flex items-center justify-center text-gray-300">
                        <svg xmlns="http://www.w3.org/2000/svg" className="w-12 h-12" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
                        </svg>
                    </div>
                }
                {/* Badge tipo */}
                <span className={`absolute top-2 left-2 text-xs font-semibold px-2 py-0.5 rounded-full ${badgeColor}`}>
                    {tipo}
                </span>
            </div>

            {/* Info */}
            <div className="p-4 flex flex-col gap-1 flex-1">
                <p className="text-sm font-semibold text-gray-800 line-clamp-2 leading-snug">{nombre}</p>
                <p className="text-xs text-gray-400">{subasta.productoRef?.ubicacion}</p>

                <div className="mt-auto pt-3 flex items-end justify-between">
                    <div>
                        <p className="text-xs text-gray-400">Precio actual</p>
                        <p className="text-lg font-bold text-gray-900">
                            ${subasta.precioActual?.toLocaleString("es-MX")}
                        </p>
                    </div>
                    <div className="text-right">
                        <p className="text-xs text-gray-400">Cierra en</p>
                        <p className={`text-sm font-semibold ${diff < 3600000 ? "text-red-500" : "text-gray-700"}`}>
                            {tiempoRestante()}
                        </p>
                    </div>
                </div>
            </div>
        </a>
    );
}

export default SubastaCard;