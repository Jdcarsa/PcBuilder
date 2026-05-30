# 🖥️ PCBuilder

Tienda de componentes para PC con armador inteligente, construida con **Blazor Server** (.NET 10) y arquitectura **Clean Architecture**.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat-square&logo=dotnet)
![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?style=flat-square&logo=blazor)
![EF Core](https://img.shields.io/badge/EF_Core-10-512BD4?style=flat-square)

---

## ✨ Funcionalidades

- 🛍️ Catálogo de componentes con filtros por categoría y búsqueda
- 🔧 Armador de PC paso a paso con validación de compatibilidad
- 🛒 Carrito de compras persistente
- 📦 Checkout sin registro obligatorio (crea cuenta automáticamente)
- 👤 Autenticación JWT con roles (Cliente / Administrador)
- 🔐 Panel de administración protegido
    - CRUD de componentes
    - Gestión de precios y stock
    - Seguimiento de pedidos
- 📱 Diseño responsive (mobile-first)

---
## 🏗️ Arquitectura

| Proyecto | Responsabilidad |
|---|---|
| `PcBuilder.Domain` | Entidades, enums y reglas de negocio |
| `PcBuilder.Application` | DTOs, interfaces y servicios de aplicación |
| `PcBuilder.Infrastructure` | EF Core, repositorios y JWT |
| `PcBuilder.Web` | Blazor Server, Minimal API endpoints y UI |

### PcBuilder.Web — estructura de componentes

| Carpeta | Contenido |
|---|---|
| `Components/Atoms` | Botones, inputs, badges, spinners |
| `Components/Molecules` | Cards, modales, filtros, filas de carrito |
| `Components/Organisms` | NavBar, CatalogoGrid, ArmadorPanel |
| `Components/Layout` | MainLayout, AdminLayout, SesionInitializer |
| `Components/Pages` | Todas las páginas de la aplicación |
| `Endpoints` | Minimal API endpoints por dominio |
| `Services` | CarritoService, SesionService, ApiClient |

## 🚀 Inicio rápido

### Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Postgres (local o en la nube)

### Instalación

```bash
# 1. Clonar el repositorio
git clone https://github.com/tu-usuario/pcbuilder.git
cd pcbuilder

# 2. Configurar la cadena de conexión en appsettings.json
# (ver sección Configuración)

# 3. Aplicar migraciones
dotnet ef database update --project PcBuilder.Infrastructure --startup-project PcBuilder.Web

# 4. Ejecutar
dotnet run --project PcBuilder.Web
```

La aplicación estará disponible en `https://localhost:5001`

---

## ⚙️ Configuración

`PcBuilder.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PcBuilderDb;Trusted_Connection=true"
  },
  "Jwt": {
    "Key": "tu-clave-secreta-minimo-32-caracteres",
    "Issuer": "PcBuilder",
    "Audience": "PcBuilderUsers",
    "ExpiresInMinutes": 60
  },
  "ApiBaseUrl": "https://localhost:5001"
}
```

---

## 👥 Usuarios de prueba

| Rol           | Email                   | Contraseña     |
|---------------|-------------------------|----------------|
| Administrador | admin@pcbuilder.com     | Admin123!      |
| Cliente       | juan@correo.com         | Cliente123!    |

> El checkout crea una cuenta automáticamente si el usuario no está registrado, usando la contraseña `PcBuilder123!`.

---

## 🔌 API Endpoints

### 🔓 Auth — `/api/auth`

| Método | Endpoint            | Descripción              | Auth       |
|--------|---------------------|--------------------------|------------|
| POST   | `/registro`         | Registrar nuevo usuario  | Anónimo    |
| POST   | `/login`            | Iniciar sesión → JWT     | Anónimo    |

**Ejemplo login:**
```json
POST /api/auth/login
{
  "email": "admin@pcbuilder.com",
  "password": "Admin123!"
}
```

---

### 🔩 Componentes — `/api/componentes`

| Método | Endpoint                    | Descripción                        | Auth          |
|--------|-----------------------------|------------------------------------|---------------|
| GET    | `/`                         | Listar todos (filtros opcionales)  | Anónimo       |
| GET    | `/?categoria=Procesador`    | Filtrar por categoría              | Anónimo       |
| GET    | `/?buscar=ryzen`            | Buscar por nombre                  | Anónimo       |
| GET    | `/{id}`                     | Obtener por Id                     | Anónimo       |
| POST   | `/`                         | Crear componente                   | 🔐 SoloAdmin  |
| PATCH  | `/{id}/precio`              | Actualizar precio                  | 🔐 SoloAdmin  |
| PATCH  | `/{id}/stock`               | Ajustar stock                      | 🔐 SoloAdmin  |
| DELETE | `/{id}`                     | Desactivar componente              | 🔐 SoloAdmin  |

**Categorías disponibles:**
`Procesador` · `PlacaMadre` · `MemoriaRam` · `Almacenamiento` · `TarjetaGrafica` · `FuentePoder` · `Gabinete` · `Refrigeracion`

---

### ⚙️ Configuraciones PC — `/api/configuraciones`

| Método | Endpoint                        | Descripción                         | Auth        |
|--------|---------------------------------|-------------------------------------|-------------|
| GET    | `/`                             | Listar configuraciones del usuario  | 🔐 JWT      |
| GET    | `/{id}`                         | Obtener por Id                      | 🔐 JWT      |
| POST   | `/`                             | Crear nueva configuración           | 🔐 JWT      |
| POST   | `/{id}/componentes`             | Agregar componente                  | 🔐 JWT      |
| DELETE | `/{id}/componentes/{compId}`    | Remover componente                  | 🔐 JWT      |
| GET    | `/{id}/validar`                 | Validar compatibilidad              | 🔐 JWT      |
| POST   | `/{id}/finalizar`               | Finalizar configuración             | 🔐 JWT      |
| DELETE | `/{id}`                         | Eliminar configuración              | 🔐 JWT      |

---

### 📦 Pedidos — `/api/pedidos`

| Método | Endpoint              | Descripción                        | Auth          |
|--------|-----------------------|------------------------------------|---------------|
| GET    | `/`                   | Listar pedidos del usuario         | 🔐 JWT        |
| GET    | `/{id}`               | Obtener pedido por Id              | 🔐 JWT        |
| POST   | `/`                   | Crear pedido                       | 🔐 JWT        |
| POST   | `/{id}/items`         | Agregar item al pedido             | 🔐 JWT        |
| POST   | `/{id}/confirmar`     | Confirmar pedido                   | 🔐 JWT        |
| POST   | `/{id}/cancelar`      | Cancelar pedido                    | 🔐 JWT        |
| POST   | `/{id}/procesar`      | Iniciar procesamiento              | 🔐 SoloAdmin  |
| POST   | `/{id}/enviar`        | Marcar como enviado                | 🔐 SoloAdmin  |
| POST   | `/{id}/entregar`      | Marcar como entregado              | 🔐 SoloAdmin  |

**Estados del pedido:** `Borrador` → `Confirmado` → `EnProcesamiento` → `Enviado` → `Entregado` / `Cancelado`

---

### 👤 Usuarios — `/api/usuarios`

| Método | Endpoint          | Descripción                          | Auth     |
|--------|-------------------|--------------------------------------|----------|
| GET    | `/me`             | Obtener perfil propio                | 🔐 JWT   |
| PUT    | `/me`             | Actualizar nombre y apellido         | 🔐 JWT   |
| PATCH  | `/me/password`    | Cambiar contraseña                   | 🔐 JWT   |

---

## 🔐 Autenticación

La API usa **JWT Bearer**. Para endpoints protegidos incluye el header:

El token se obtiene del endpoint `/api/auth/login` y expira según la configuración (`ExpiresInMinutes`).

---

## 🛠️ Stack tecnológico

| Capa            | Tecnología                         |
|-----------------|------------------------------------|
| Frontend        | Blazor Server (.NET 10)            |
| UI              | Bootstrap 5 + Bootstrap Icons      |
| Backend         | ASP.NET Core Minimal APIs          |
| ORM             | Entity Framework Core 10           |
| Base de datos   | Postgres Sql                       |
| Autenticación   | JWT Bearer                         |
| Arquitectura    | Clean Architecture + Atomic Design |

---

## 📄 Licencia

MIT — libre para uso personal y comercial.