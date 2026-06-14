import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Login from './pages/Login'
import Signup from './pages/Signup'
import SignupVendedor from './pages/SignupVendedor'
import DetallesUsuario from './pages/DetallesUsuario'
import Home from './pages/Home'
import Navbar from './components/Navbar'
import ActualizarDatos from './pages/ActualizarDatos'
import CambiarContraseña from './pages/CambiarContraseña'
import CrearSubasta from './pages/CrearSubasta'
import Resultados from './pages/Resultados'

function App() {
    return (
        <BrowserRouter>
            <Routes>
                {/* Rutas SIN navbar */}
                <Route path="/" element={<Login />} />
                <Route path="/signup" element={<Signup />} />
                <Route path="/signup-vendedor" element={<SignupVendedor />} />

                {/* Rutas CON navbar */}
                <Route path="/*" element={
                    <>
                        <Navbar />
                        <div className="pt-14">
                            <Routes>
                                <Route path="/" element={<Home />} />
                                <Route path="/signup" element={<Signup />} />
                                <Route path="/signup-vendedor" element={<SignupVendedor />} />
                                <Route path="/home" element={<Home />} />
                                <Route path="/resultados" element={<Resultados />} />
                                <Route path="/detalles-usuario" element={<DetallesUsuario />} />
                                <Route path="/actualizar-datos" element={<ActualizarDatos />} />
                                <Route path="/cambiar-contrasenia" element={<CambiarContraseña />} />
                                <Route path="/CrearSubasta" element={<CrearSubasta />} />
                            </Routes>
                        </div>
                    </>
                } />
            </Routes>
        </BrowserRouter>
    )
}

export default App