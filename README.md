Prueba Técnica - .NET Core + React

Este proyecto consiste en una API RESTful construida en .NET Core con autenticación basada en JWT, manejo de roles (admin y usuario normal), control de sesión, y entidades relacionadas al
usuario autenticado. La API es consumida por una SPA construida en React (solo con fines funcionales).

Tecnologías Utilizadas:
Backend
.NET 8
Entity Framework Core
SQL Server
JWT (Json Web Token)
Swagger

Frontend
React (SPA)
Vite

Requisitos Previos
.NET 6 o superior instalado
SQL Server
Node.js y npm

Configuración del Backend
Clona el repositorio
Configura el archivo appsettings.json en el proyecto del backend. Debes configurar el Key, junto a su Issuer y Audicence, te dejo ese de ejemplo. 
Este archivo debe incluir:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR_SQL;Database=PruebaTecnicaZoco;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "MySuperSecretKeyWithAtLeast32Characters123!", 
    "Issuer": "PruebaTecnicaZoco",
    "Audience": "PruebaTecnicaZoco"
  }
}

Asegúrate de que tanto este appsettings.json como en el proyectos de DbContext estén correctamente configurados.

Abre la consola de Administrador de Paquetes en Visual Studio y ejecuta:
Add-Migration InitialCreate
Update-Database

Esto aplicará las migraciones y creará la base de datos en SQL Server.

Ejecuta el proyecto backend (por defecto en http://localhost:5186).

Configuración del Frontend
Ve al directorio del frontend (SPA con React).
Crea un archivo .env y configura la URL del backend:
VITE_API_BASE_URL=http://localhost:5186
Instala las dependencias y ejecuta el frontend:
npm install
npm run dev

Endpoints Clave:

Autenticación
POST /auth/login - Devuelve JWT e inicia sesión
POST /auth/logout - Finaliza sesión

Protección con JWT
El token debe enviarse por header:
Authorization: Bearer <token>

Roles
1- Admin
Crear, ver, editar y eliminar usuarios, estudios y direcciones
2-Usuario normal
Ver y editar su perfil
Gestionar solo sus propios estudios y direcciones

Características Generales
Control de sesión (registro de inicio/cierre en tabla SessionLogs)
Protección de endpoints con [Authorize]
Validación de propiedad de recursos para usuarios normales
Visualización de la API mediante Swagger (/swagger)
Estructura del Proyecto
Controllers/ - Endpoints de la API
Services/ - Lógica de negocio
Repositories/ - Acceso a datos
Data/ - Configuración del DbContext y migraciones

Notas Finales

Este proyecto es funcional y está orientado a demostrar el manejo de roles, autenticación JWT y relaciones entre entidades.
El diseño del frontend es mínimo y cumple solo con el objetivo de consumir la API.

Autor
Fernando Baunaly
