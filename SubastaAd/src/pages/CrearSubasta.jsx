import { useState } from 'react'
import Navbar from '../components/Navbar'

function CrearSubasta() {
    const idUsuario = localStorage.getItem('idUsuario')
    const token = localStorage.getItem('token')

    const opcionesCategoria = [
        { valor: 1, texto: 'Vehículo' },
        { valor: 2, texto: 'Inmueble' },
        { valor: 3, texto: 'Electrónicos' },
        { valor: 4, texto: 'Arte y coleccionables' },
        { valor: 5, texto: 'Antigüedades' },
        { valor: 6, texto: 'Ropa y accesorios' },
        { valor: 7, texto: 'Artículos deportivos' },
        { valor: 8, texto: 'Libros' },
        { valor: 9, texto: 'Juguetes' },
        { valor: 10, texto: 'Contenidos digitales' },
        { valor: 11, texto: 'Entrada a eventos' }
    ]

    const opcionesCondicion = [
        { valor: 1, texto: 'Nuevo' },
        { valor: 2, texto: 'Usado' },
        { valor: 3, texto: 'Reacondicionado' }
    ]

    const opcionesTipoSubasta = [
        { valor: 1, texto: 'Inglesa' },
        { valor: 2, texto: 'Holandesa' },
        { valor: 3, texto: 'Sellada' }
    ]

    const [form, setForm] = useState({
        Nombre: '', Descripcion: '', Ubicacion: '', CveCategoria: 3, CveCondicion: 1,
        Marca: '', Modelo: '', Anio: '', Kilometraje: '', NumeroSerie: '', UrlDocumentacionVehiculo: '',
        SuperficieTerreno: '', SuperficieConstruida: '', NumeroHabitaciones: '', UrlDocumentacionInmueble: '',
        PrecioInicial: '', PrecioMinimo: '', Incremento: '', FechaInicio: '', FechaFinal: '', CveTipoSubasta: 1
    })

    const [fotos, setFotos] = useState([])
    const [loading, setLoading] = useState(false)

    // --- LÓGICA PARA RESTRICCIÓN DE FECHAS ---
    const getFechasMinimas = () => {
        const hoy = new Date()
        const tzOffset = hoy.getTimezoneOffset() * 60000 // Ajuste de zona horaria local

        // Fecha actual
        const minInicio = new Date(hoy - tzOffset).toISOString().slice(0, 16)
        
        // Fecha actual + 1 día
        const manana = new Date(hoy)
        manana.setDate(manana.getDate() + 1)
        const minFinal = new Date(manana - tzOffset).toISOString().slice(0, 16)

        return { minInicio, minFinal }
    }

    const { minInicio, minFinal } = getFechasMinimas()
    // -----------------------------------------

    const handleChange = (e) => {
        const { name, value } = e.target
        if (['CveCategoria', 'CveCondicion', 'CveTipoSubasta'].includes(name)) {
            setForm({ ...form, [name]: parseInt(value) })
        } else {
            setForm({ ...form, [name]: value })
        }
    }

    const handleFotos = (e) => {
        if (e.target.files && e.target.files.length > 0) {
            setFotos(Array.from(e.target.files))
        }
    }

    const handleSubmit = async (e) => {
        e.preventDefault()

        if (!idUsuario || !token) {
            alert('No se ha iniciado sesión correctamente')
            return
        }

        // --- VALIDACIÓN DE FECHAS ANTES DE ENVIAR ---
        if (form.FechaInicio < minInicio) {
            alert('La fecha de inicio no puede ser menor a la fecha y hora actual.')
            return
        }

        if (form.FechaFinal < minFinal) {
            alert('La fecha final debe ser al menos 1 día mayor a la fecha actual.')
            return
        }
        // --------------------------------------------

        setLoading(true)

        try {
            const formData = new FormData()

            // Datos básicos
            formData.append('Nombre', form.Nombre)
            formData.append('Descripcion', form.Descripcion)
            formData.append('Ubicacion', form.Ubicacion)
            formData.append('CveCategoria', form.CveCategoria)
            formData.append('CveCondicion', form.CveCondicion)
            formData.append('CveUsuario', parseInt(idUsuario))

            // Datos de subasta
            formData.append('PrecioInicial', form.PrecioInicial)
            formData.append('FechaInicio', form.FechaInicio)
            formData.append('FechaFinal', form.FechaFinal)
            formData.append('CveTipoSubasta', form.CveTipoSubasta)

            if (parseInt(form.CveTipoSubasta) === 1 && form.Incremento) formData.append('Incremento', form.Incremento)
            if (parseInt(form.CveTipoSubasta) === 2 && form.PrecioMinimo) formData.append('PrecioMinimo', form.PrecioMinimo)

            // Categorías específicas
            if (parseInt(form.CveCategoria) === 1) {
                if (form.Marca) formData.append('Marca', form.Marca)
                if (form.Modelo) formData.append('Modelo', form.Modelo)
                if (form.Anio) formData.append('Anio', form.Anio)
                if (form.Kilometraje) formData.append('Kilometraje', form.Kilometraje)
                if (form.NumeroSerie) formData.append('NumeroSerie', form.NumeroSerie)
                if (form.UrlDocumentacionVehiculo) formData.append('UrlDocumentacionVehiculo', form.UrlDocumentacionVehiculo)
            }

            if (parseInt(form.CveCategoria) === 2) {
                if (form.SuperficieTerreno) formData.append('SuperficieTerreno', form.SuperficieTerreno)
                if (form.SuperficieConstruida) formData.append('SuperficieConstruida', form.SuperficieConstruida)
                if (form.NumeroHabitaciones) formData.append('NumeroHabitaciones', form.NumeroHabitaciones)
                if (form.UrlDocumentacionInmueble) formData.append('UrlDocumentacionInmueble', form.UrlDocumentacionInmueble)
            }

            // Fotos
            if (fotos.length > 0) {
                fotos.forEach((foto) => {
                    formData.append('Fotos', foto, foto.name)
                })
            }

            const response = await fetch('http://localhost:5288/api/Subasta/crear', {
                method: 'POST',
                headers: { 'Authorization': `Bearer ${token}` },
                body: formData
            })

            if (response.ok) {
                const data = await response.json()
                alert(`Subasta creada correctamente. ID: ${data.idSubasta}`)
                window.location.href = '/home'
            } else {
                const error = await response.text()
                alert(`Error: ${error}`)
            }

        } catch (error) {
            alert('No se pudo conectar con el servidor')
        } finally {
            setLoading(false)
        }
    }

    // Clases CSS reutilizables para mantener consistencia
    const inputClasses = "w-full border border-gray-300 rounded-md p-3 text-gray-700 focus:outline-none focus:ring-2 focus:ring-gray-400 focus:border-transparent transition-all bg-gray-50 focus:bg-white"
    const labelClasses = "block text-sm font-semibold text-gray-700 mb-1"
    const cardClasses = "bg-white rounded-lg shadow-sm border border-gray-200 p-6 md:p-8 mb-6"

    return (
        <>
            <Navbar />

            <div className="min-h-screen bg-[#ebebeb] pt-28 pb-12 px-4 sm:px-6">
                <div className="max-w-3xl mx-auto">
                    
                    <div className="mb-8">
                        <h1 className="text-2xl font-bold text-gray-900">Cuéntanos sobre tu artículo</h1>
                        <p className="text-gray-600 mt-1">Completa los datos para publicar tu subasta.</p>
                    </div>

                    <form onSubmit={handleSubmit}>
                        
                        {/* SECCIÓN 1: Datos Principales */}
                        <div className={cardClasses}>
                            <h2 className="text-lg font-bold text-gray-800 mb-5 border-b pb-2">Datos principales</h2>
                            <div className="space-y-5">
                                <div>
                                    <label className={labelClasses}>Título de la publicación *</label>
                                    <input type="text" name="Nombre" value={form.Nombre} onChange={handleChange} required className={inputClasses} placeholder="Ej. iPhone 13 Pro Max 256GB" />
                                </div>
                                <div>
                                    <label className={labelClasses}>Descripción *</label>
                                    <textarea name="Descripcion" value={form.Descripcion} onChange={handleChange} required rows="4" className={inputClasses} placeholder="Describe los detalles más importantes de tu artículo..." />
                                </div>
                                <div>
                                    <label className={labelClasses}>Ubicación *</label>
                                    <input type="text" name="Ubicacion" value={form.Ubicacion} onChange={handleChange} required className={inputClasses} placeholder="Ej. Ciudad de México, CDMX" />
                                </div>
                            </div>
                        </div>

                        {/* SECCIÓN 2: Categoría y Estado */}
                        <div className={cardClasses}>
                            <h2 className="text-lg font-bold text-gray-800 mb-5 border-b pb-2">Clasificación</h2>
                            <div className="grid md:grid-cols-2 gap-5">
                                <div>
                                    <label className={labelClasses}>Categoría</label>
                                    <select name="CveCategoria" value={form.CveCategoria} onChange={handleChange} className={inputClasses}>
                                        {opcionesCategoria.map(opcion => (
                                            <option key={opcion.valor} value={opcion.valor}>{opcion.texto}</option>
                                        ))}
                                    </select>
                                </div>
                                <div>
                                    <label className={labelClasses}>Condición</label>
                                    <select name="CveCondicion" value={form.CveCondicion} onChange={handleChange} className={inputClasses}>
                                        {opcionesCondicion.map(opcion => (
                                            <option key={opcion.valor} value={opcion.valor}>{opcion.texto}</option>
                                        ))}
                                    </select>
                                </div>
                            </div>
                        </div>

                        {/* SECCIÓN 3: Imágenes */}
                        <div className={cardClasses}>
                            <h2 className="text-lg font-bold text-gray-800 mb-5 border-b pb-2">Fotografías</h2>
                            <div className="flex flex-col items-start gap-3">
                                <p className="text-sm text-gray-600">Sube fotos claras y con buena iluminación para atraer más ofertas.</p>
                                
                                <label className="cursor-pointer inline-flex items-center justify-center px-6 py-3 border-2 border-dashed border-gray-300 rounded-md text-gray-700 bg-gray-50 hover:bg-gray-100 hover:border-gray-400 transition-all w-full sm:w-auto">
                                    <svg className="w-5 h-5 mr-2 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12"></path></svg>
                                    <span className="font-medium">Subir Imagen</span>
                                    <input type="file" multiple accept=".jpg,.jpeg,.png,.webp" onChange={handleFotos} className="hidden" />
                                </label>

                                {fotos.length > 0 && (
                                    <div className="mt-3 w-full bg-gray-50 p-3 rounded-md border border-gray-200">
                                        <p className="text-sm font-semibold text-gray-700 mb-2">{fotos.length} archivo(s) seleccionado(s):</p>
                                        <ul className="text-sm text-gray-600 space-y-1 list-disc pl-5">
                                            {fotos.map((f, i) => <li key={i} className="truncate">{f.name}</li>)}
                                        </ul>
                                    </div>
                                )}
                            </div>
                        </div>

                        {/* SECCIÓN DINÁMICA: Vehículos */}
                        {parseInt(form.CveCategoria) === 1 && (
                            <div className={cardClasses}>
                                <h2 className="text-lg font-bold text-gray-800 mb-5 border-b pb-2">Características del Vehículo</h2>
                                <div className="grid md:grid-cols-2 gap-5">
                                    <div><label className={labelClasses}>Marca</label><input type="text" name="Marca" value={form.Marca} onChange={handleChange} className={inputClasses} /></div>
                                    <div><label className={labelClasses}>Modelo</label><input type="text" name="Modelo" value={form.Modelo} onChange={handleChange} className={inputClasses} /></div>
                                    <div><label className={labelClasses}>Año</label><input type="number" name="Anio" value={form.Anio} onChange={handleChange} className={inputClasses} /></div>
                                    <div><label className={labelClasses}>Kilometraje</label><input type="number" name="Kilometraje" value={form.Kilometraje} onChange={handleChange} className={inputClasses} /></div>
                                    <div><label className={labelClasses}>Número de Serie</label><input type="number" name="NumeroSerie" value={form.NumeroSerie} onChange={handleChange} className={inputClasses} /></div>
                                    <div><label className={labelClasses}>Documentación (URL)</label><input type="text" name="UrlDocumentacionVehiculo" value={form.UrlDocumentacionVehiculo} onChange={handleChange} className={inputClasses} placeholder="https://..." /></div>
                                </div>
                            </div>
                        )}

                        {/* SECCIÓN DINÁMICA: Inmuebles */}
                        {parseInt(form.CveCategoria) === 2 && (
                            <div className={cardClasses}>
                                <h2 className="text-lg font-bold text-gray-800 mb-5 border-b pb-2">Detalles del Inmueble</h2>
                                <div className="grid md:grid-cols-2 gap-5">
                                    <div><label className={labelClasses}>Superficie Terreno (m²)</label><input type="number" step="0.01" name="SuperficieTerreno" value={form.SuperficieTerreno} onChange={handleChange} className={inputClasses} /></div>
                                    <div><label className={labelClasses}>Superficie Construida (m²)</label><input type="number" step="0.01" name="SuperficieConstruida" value={form.SuperficieConstruida} onChange={handleChange} className={inputClasses} /></div>
                                    <div><label className={labelClasses}>Número de Habitaciones</label><input type="number" name="NumeroHabitaciones" value={form.NumeroHabitaciones} onChange={handleChange} className={inputClasses} /></div>
                                    <div><label className={labelClasses}>Documentación (URL)</label><input type="text" name="UrlDocumentacionInmueble" value={form.UrlDocumentacionInmueble} onChange={handleChange} className={inputClasses} placeholder="https://..." /></div>
                                </div>
                            </div>
                        )}

                        {/* SECCIÓN 4: Condiciones de Subasta */}
                        <div className={cardClasses}>
                            <h2 className="text-lg font-bold text-gray-800 mb-5 border-b pb-2">Configuración de la Subasta</h2>
                            
                            <div className="grid md:grid-cols-2 gap-5 mb-5">
                                <div>
                                    <label className={labelClasses}>Tipo de Subasta</label>
                                    <select name="CveTipoSubasta" value={form.CveTipoSubasta} onChange={handleChange} className={inputClasses}>
                                        {opcionesTipoSubasta.map(opcion => (
                                            <option key={opcion.valor} value={opcion.valor}>{opcion.texto}</option>
                                        ))}
                                    </select>
                                </div>
                                <div>
                                    <label className={labelClasses}>Precio Inicial *</label>
                                    <div className="relative">
                                        <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-gray-500">$</span>
                                        <input type="number" step="0.01" name="PrecioInicial" value={form.PrecioInicial} onChange={handleChange} required className={`${inputClasses} pl-8`} placeholder="0.00" />
                                    </div>
                                </div>
                            </div>

                            {parseInt(form.CveTipoSubasta) === 1 && (
                                <div className="mb-5">
                                    <label className={labelClasses}>Incremento mínimo</label>
                                    <div className="relative">
                                        <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-gray-500">$</span>
                                        <input type="number" step="0.01" name="Incremento" value={form.Incremento} onChange={handleChange} className={`${inputClasses} pl-8`} placeholder="0.00" />
                                    </div>
                                </div>
                            )}

                            {parseInt(form.CveTipoSubasta) === 2 && (
                                <div className="mb-5">
                                    <label className={labelClasses}>Precio Mínimo</label>
                                    <div className="relative">
                                        <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-gray-500">$</span>
                                        <input type="number" step="0.01" name="PrecioMinimo" value={form.PrecioMinimo} onChange={handleChange} className={`${inputClasses} pl-8`} placeholder="0.00" />
                                    </div>
                                </div>
                            )}

                            <div className="grid md:grid-cols-2 gap-5">
                                <div>
                                    <label className={labelClasses}>Fecha de Inicio *</label>
                                    <input 
                                        type="datetime-local" 
                                        name="FechaInicio" 
                                        value={form.FechaInicio} 
                                        onChange={handleChange} 
                                        min={minInicio} 
                                        required 
                                        className={inputClasses} 
                                    />
                                </div>
                                <div>
                                    <label className={labelClasses}>Fecha de Finalización *</label>
                                    <input 
                                        type="datetime-local" 
                                        name="FechaFinal" 
                                        value={form.FechaFinal} 
                                        onChange={handleChange} 
                                        min={minFinal} 
                                        required 
                                        className={inputClasses} 
                                    />
                                </div>
                            </div>
                        </div>

                        {/* BOTÓN FINAL */}
                        <div className="flex justify-end mt-8">
                            <button
                                type="submit"
                                disabled={loading}
                                className="bg-green-900 text-white font-semibold px-8 py-4 rounded-md hover:bg-green-800 transition-colors disabled:bg-green-400 disabled:cursor-not-allowed w-full sm:w-auto shadow-md"
                            >
                                {loading ? 'Publicando...' : 'Publicar Subasta'}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </>
    )
}

export default CrearSubasta