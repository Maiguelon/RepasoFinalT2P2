# 📝 EXAMEN SIMULACRO  
## Sistema de Gestión de Mangas (ASP.NET MVC)

---

## Contexto

El sistema utiliza una base de datos **SQLite preexistente**.  
Se requiere implementar el **ABM (Alta, Baja y Modificación)** de la entidad principal, asegurando el uso de **Inyección de Dependencias**, **ViewModels** y **mapeo manual**.

---

## 1. Entidades y Enums (Modelos)

### Enum `Demografia`

Crear un enum llamado `Demografia` con los siguientes valores:

```csharp
public enum Demografia
{
    Shonen = 1,
    Seinen = 2,
    Shojo = 3,
    Josei = 4
}

## Entidad `Manga`

Crear la clase `Manga` con los siguientes campos:

| Campo            | Tipo        |
|------------------|-------------|
| Id               | int         |
| Titulo           | string      |
| TomosPublicados  | int         |
| Demografia       | Demografia  |

---

## 2. ViewModels

Debe respetarse estrictamente la separación de datos mediante el uso de ViewModels.

---

### `MangaIndexViewModel`

Utilizado para la vista de listado.

Campos requeridos:
- Id  
- Titulo  
- TomosPublicados  
- Demografia  

---

### `MangaCreateViewModel`

Utilizado para el Alta de mangas.

Validaciones obligatorias:

| Campo            | Reglas |
|------------------|--------|
| Titulo           | Obligatorio, entre 2 y 150 caracteres |
| TomosPublicados  | Obligatorio, rango entre 1 y 500 |
| Demografia       | Obligatoria |

Requisitos adicionales:
- Debe incluir un `SelectList` para cargar el selector desplegable de Demografía en la vista.

---

### `MangaUpdateViewModel`

Utilizado para la Modificación de mangas.

Requisitos:
- Debe contener los mismos campos y validaciones que `MangaCreateViewModel`
- Debe incluir el campo `Id` como obligatorio

---

## 3. Repositorio (Acceso a Datos)

Crear la interfaz `IMangaRepository` y su implementación `MangaRepository`.

---

### Métodos requeridos (CRUD)

| Método   | Descripción |
|----------|-------------|
| GetAll   | Obtiene todos los registros |
| GetById  | Obtiene un registro por Id |
| Add      | Agrega un nuevo registro |
| Update   | Modifica un registro existente |
| Delete   | Elimina un registro |

---

### Requisitos técnicos

- El valor del enum `Demografia` debe almacenarse y recuperarse correctamente desde la base de datos SQLite.
- Deben utilizarse comandos parametrizados para las operaciones de base de datos.

---

## 4. Controlador y Vistas

### `MangaController`

---

### Seguridad

- El acceso al controlador completo debe estar restringido mediante inyección del servicio de autenticación.
- La acción `Index` debe ser accesible para cualquier usuario logueado.
- Las acciones de Alta, Baja y Modificación deben ser accesibles únicamente para el rol **Admin**.
- Si un usuario sin el rol requerido intenta acceder a dichas acciones, debe ser redirigido a la vista **Acceso Denegado**.

---

### Mapeo de datos

- Todos los envíos de datos hacia las vistas y desde los formularios deben realizarse exclusivamente mediante ViewModels.
- El mapeo entre Entidades y ViewModels debe realizarse de forma manual.

---

### Desplegables

- En las vistas de Alta (Create) y Modificación (Update), la selección de la Demografía debe realizarse obligatoriamente mediante un selector desplegable (`<select>`).
- Si ocurre un error de validación (`ModelState.IsValid == false`), la vista debe recargarse manteniendo las opciones del desplegable intactas.