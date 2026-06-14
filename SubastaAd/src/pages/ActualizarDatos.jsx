import {useState, useEffect} from 'react'

function ActualizarDatos() {
    const token = localStorage.getItem('token')
    const id = localStorage.getItem('idUsuario')

    const [form, setForm] = useState({
        nombre: '',
        apellidoPaterno: '',
        apellidoMaterno: '',
        correo: '',
    })

    useEffect(() => {
        if (!token || !id) {
            window.location.href = '/'; return
        }

        fetch(`http://localhost:5288/api/usuario/${id}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        })
        .then(res => res.json())
        .then(data => setForm({
            nombre: data.nombre,
            apellidoPaterno: data.apellidoPaterno,
            apellidoMaterno: data.apellidoMaterno,
            correo: data.correo
        }))
        .catch(() => alert('No se pudo cargar la información del usuario'))
    }, [])

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value })
    }

    const handleSubmit = async (e) => {
        e.preventDefault()

        try {
            const response = await fetch(`http://localhost:5288/api/usuario/${id}`, {
                method: 'PUT',
                headers: { 
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ idUsuario: parseInt(id), ...form })
            })

            if (response.ok) {
                alert('Información actualizada correctamente')
                window.location.href = '/detalles-usuario'
            } else {
                alert('Error al actualizar la información')
            }
        } catch (error) {
            alert('No se pudo conectar con el servidor')
        }
    }

    return (
        <div className="min-h-screen bg-gray-100 flex flex-col justify-center items-center py-10 px-4">
            <form
                onSubmit={handleSubmit}
                className="bg-white w-full max-w-md rounded-xl shadow-lg p-8 flex flex-col gap-4"
            >
                <h2 className="text-2xl font-semibold text-gray-800">Actualizar datos</h2>
                <div>
                    <label className="w-full font-medium text-gray-700">Nombre</label>
                    <input
                        type="text"
                        name="nombre"
                        value={form.nombre}
                        onChange={handleChange}
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:outline-none"
                    />
                </div>

                <div>
                    <label className="w-full font-medium text-gray-700">Apellido paterno</label>
                    <input
                        type="text"
                        name="apellidoPaterno"
                        value={form.apellidoPaterno}
                        onChange={handleChange}
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:outline-none"
                    />
                </div>

                <div>
                    <label className="w-full font-medium text-gray-700">Apellido materno</label>
                    <input
                        type="text"
                        name="apellidoMaterno"
                        value={form.apellidoMaterno}
                        onChange={handleChange}
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:outline-none"
                    />
                </div>

                <div>
                    <label className="w-full font-medium text-gray-700">Correo electrónico</label>
                    <input
                        type="email"
                        name="correo"
                        value={form.correo}
                        onChange={handleChange}
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:outline-none"
                    />
                </div>

                <div className="flex gap-3 mt-2">
                    <button
                        type="button"
                        onClick={() => window.location.href = '/detalles-usuario'}
                        className="bg-gray-400 text-white px-4 py-2 rounded-lg hover:opacity-75 transition-all"
                    >
                        Cancelar
                    </button>
                    <button
                        type="submit"
                        className="w-full bg-blue-500 text-white py-2 px-4 rounded-lg hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500"
                    >
                        Guardar cambios
                    </button>
                </div>
            </form>
        </div>
    )
}

export default ActualizarDatos