TheHouseLogin – Sistema de Ventas y Gestión de Productos

 TheHouseLogin es una aplicación de escritorio desarrollada en WPF (C#) diseñada para la gestión interna del negocio THE HOUSE BEBIDAS.
 Incluye sistema de inicio de sesión, panel principal moderno, CRUD de productos y registro de ventas.

🚀 Tecnologías utilizadas

 C# – .NET 8 / WPF
 
 SQL Server LocalDB mediante archivo .mdf
 
 Entity Framework Core
 
 Arquitectura por capas:
 
 UI (WPF)
 
 Data
 
 Domain
 
 Services

📌 Funcionalidades principales
🔐 Autenticación

 Inicio de sesión seguro
 
 Ventana de login con diseño moderno

📦 Gestión de Productos

 Crear, editar y eliminar productos
 
 Campos: Id, Nombre, Precio, Stock
 
 Vista principal con tabla y buscador

🛒 Registro de Ventas

 Selección de productos
 
 Cálculo automático de totales
 
 Registro y almacenamiento de ventas

📊 Dashboard Moderno

 Totales del día
 
 Últimas ventas
 
 Navegación amigable

📂 Estructura del Proyecto
/TheHouseLogin
 ├── TheHouseLogin.sln
 ├── TheHouseLogin.UI
 ├── TheHouseLogin.Data
 ├── TheHouseLogin.Domain
 ├── TheHouseLogin.Services
 └── README.md

⚙️ Configuración de Base de Datos

 El proyecto utiliza LocalDB con un archivo Database.mdf.
 
 Desde Visual Studio, la base se adjunta automáticamente
