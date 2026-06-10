# SubastaApi — Guía de instalación para el equipo de Frontend

## Requisitos previos

Antes de comenzar asegúrate de tener instalado lo siguiente:

### 1. .NET SDK
Descarga e instala la versión más reciente desde:
👉 https://dotnet.microsoft.com/es-es/

Verifica la instalación:
```bash
dotnet --version
```

### 2. SQL Server
Descarga e instala SQL Server desde:
👉 https://www.microsoft.com/es-es/sql-server/sql-server-downloads

Se recomienda instalar la versión **Developer** (es gratuita).

### 3. Git
Descarga e instala Git desde:
👉 https://git-scm.com/downloads

Verifica la instalación:
```bash
git --version
```

---

## Clonar el repositorio

Ejecuta este comando en la carpeta donde quieras guardar el proyecto:

```bash
git clone https://github.com/ZakuroByte/SubastaApi.git
```

Entra a la carpeta del proyecto:

```bash
cd SubastaApi
```

---

## Configuración antes de correr el proyecto

### 1. Crear el archivo de configuración local

Crea un archivo llamado `appsettings.Development.json` en la raíz del proyecto con el siguiente contenido:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SubastaDb;User Id=sa;Password=tupassword;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "una-clave-secreta-larga-de-minimo-32-caracteres",
    "Issuer": "SubastaApi",
    "Audience": "SubastaApi"
  }
}
```

> ⚠️ Reemplaza `tupassword` con la contraseña de tu SQL Server local.
> ⚠️ Este archivo **no está en el repositorio** por seguridad, debes crearlo manualmente.

---

### 2. Restaurar los paquetes de NuGet

```bash
dotnet restore
```

### 3. Aplicar las migraciones a la base de datos

Este comando crea la base de datos y todas las tablas automáticamente:

```bash
dotnet ef database update
```

Si te dice que no reconoce el comando `dotnet ef`, instala la herramienta con:

```bash
dotnet tool install --global dotnet-ef
```

Y vuelve a intentar el comando anterior.

---

## Correr el proyecto

```bash
dotnet run
```

La API estará disponible en:
```
http://localhost:5288
```

Para ver todos los endpoints disponibles entra a:
```
http://localhost:5288/swagger
```

---

## Actualizar el proyecto

Cada vez que haya cambios nuevos en el repositorio ejecuta:

```bash
git pull
```

Si hubo cambios en la base de datos (nuevas migraciones), vuelve a correr:

```bash
dotnet ef database update
```

---

## Comandos de referencia rápida

| Comando | Descripción |
|---|---|
| `git pull` | Descargar los últimos cambios del repositorio |
| `dotnet restore` | Restaurar paquetes de NuGet |
| `dotnet run` | Correr la API |
| `dotnet ef database update` | Aplicar migraciones a la base de datos |
| `dotnet build` | Compilar el proyecto sin correrlo |

---

## Notas importantes

- No modifiques ni subas cambios al repositorio, solo descarga actualizaciones con `git pull`
- Si tienes errores al correr el proyecto verifica que SQL Server esté corriendo en tu computadora
- El archivo `appsettings.Development.json` es personal de cada quien, no lo compartas