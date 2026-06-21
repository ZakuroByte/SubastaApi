import { useState } from 'react'

function Login() {
  const [correo, setCorreo] = useState('')
  const [contrasenia, setContrasenia] = useState('')
  const [visible, setVisible] = useState(false)

  const handleSubmit = async (e) => {
    e.preventDefault()

    if (correo === '' || contrasenia === '') {
      alert('Por favor llena todos los campos')
      return
    }

    try {
      const response = await fetch('http://localhost:5288/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ correo, contrasenia })
      })

      if (response.ok) {
        const data = await response.json()
        console.log(data)
        localStorage.setItem('token', data.token)
        localStorage.setItem('idUsuario', data.usuario.idUsuario)
        localStorage.setItem('tipoUsuario', data.usuario.tipoUsuario)
        window.location.href = '/'
      } else {
        alert('Correo o contraseña incorrectos')
      }
    } catch (error) {
      alert('No se pudo conectar con el servidor')
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <form
        onSubmit={handleSubmit}
        className="bg-white w-80 flex flex-col items-center gap-2 py-8 px-6 rounded-xl shadow-lg"
      >
        <img src="/logo.png" alt="Logo" className="w-24 h-24 mb-2" />

        <h2 className="text-lg font-semibold text-gray-700">Iniciar sesión</h2>

        <label className="w-full font-medium text-gray-700">Correo</label>
        <input
          type="email"
          placeholder="correo@ejemplo.com"
          value={correo}
          onChange={(e) => setCorreo(e.target.value)}
          required
          className="w-full border border-gray-300 rounded px-3 py-2 outline-none focus:border-blue-500"
        />

        <label className="w-full font-medium text-gray-700">Contraseña</label>
        <input
          type={visible ? 'text' : 'password'}
          placeholder="Contraseña"
          value={contrasenia}
          onChange={(e) => setContrasenia(e.target.value)}
          required
          className="w-full border border-gray-300 rounded px-3 py-2 outline-none focus:border-blue-500"
        />

        <div className="w-full flex items-center gap-2 mt-1">
          <input
            type="checkbox"
            id="visible"
            checked={visible}
            onChange={() => setVisible(!visible)}
          />
          <label htmlFor="visible" className="text-sm text-gray-600">
            Mostrar contraseña
          </label>
        </div>

        <button
          type="submit"
          className="w-full bg-blue-600 text-white font-medium py-2 rounded-full mt-3 hover:opacity-75 transition-all"
        >
          Iniciar sesión
        </button>

        <p className="text-sm text-gray-500 mt-2">
          ¿No tienes cuenta?{' '}
          <a href="/signup" className="text-blue-600 font-medium hover:underline">
            Regístrate
          </a>
        </p>
      </form>
    </div>
  )
}

export default Login