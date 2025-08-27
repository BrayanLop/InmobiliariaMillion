# InmobiliariaMillion - Gu�a de Implementaci�n y Ejecuci�n

## Descripci�n del Proyecto
Sistema de gesti�n inmobiliaria desarrollado en .NET 8 con MongoDB como base de datos, para administrar propiedades de forma eficiente.

## Requisitos T�cnicos
- .NET 8 SDK
- MongoDB 6.0 o superior
- Visual Studio 2022 (recomendado)
- Espacio en disco: m�nimo 500 MB
- RAM: m�nimo 4 GB

## Estructura del Proyecto
InmobiliariaMillion/
InmobiliariaMillion.API/# Capa de presentaci�n (API REST)
InmobiliariaMillion.Aplicacion/# Capa de aplicaci�n (Servicios, DTOs) InmobiliariaMillion.Dominio/# Capa de dominio (Entidades, Interfaces) InmobiliariaMillion.Infraestructura/# Capa de infraestructura (Repositorios, Configuraci�n)

## Configuraci�n Paso a Paso

### 1. Base de Datos
1. Instale MongoDB siguiendo la [documentaci�n oficial](https://www.mongodb.com/docs/manual/installation/).
2. Cree una base de datos llamada `InmobiliariaMillion`.

### 2. Configuraci�n del Proyecto
1. Clone el repositorio: `git clone https://github.com/tu-usuario/InmobiliariaMillion.git`
2. Abra la soluci�n en Visual Studio 2022
3. Restaure los paquetes NuGet: `dotnet restore`

### 3. Configuraci�n de appsettings.json
Modifique el archivo `appsettings.json` en la capa API:
{ "MongoSettings": { "ConnectionString": "mongodb://localhost:27017", "DatabaseName": "InmobiliariaMillion" }, "JwtSettings": { "SecretKey": "ClaveSecretaParaGenerarToken", "Issuer": "InmobiliariaMillion", "Audience": "Users", "DurationInMinutes": 60 }, "CorsSettings": { "AllowedOrigins": ["http://localhost:3000", "https://inmobiliaria-million-frontend.com"] }, "ImageSettings": { "StoragePath": "wwwroot/images/propiedades", "MaxSizeInMB": 5, "AllowedExtensions": [".jpg", ".jpeg", ".png"] }, "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } } }


### 4. Configuraciones en Program.cs
El archivo `Program.cs` contiene todas las configuraciones necesarias para la ejecuci�n:
// Configuraci�n de MongoDB builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings")); builder.Services.AddSingleton<IMongoClient>(sp => { var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value; return new MongoClient(settings.ConnectionString); }); builder.Services.AddSingleton<IMongoDatabase>(sp => { var client = sp.GetRequiredService<IMongoClient>(); var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value; return client.GetDatabase(settings.DatabaseName); });
// Configuraci�n de CORS builder.Services.AddCors(options => { var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>(); options.AddPolicy("AllowSpecificOrigins", policy => policy .WithOrigins(corsSettings.AllowedOrigins.ToArray()) .AllowAnyHeader() .AllowAnyMethod() .AllowCredentials()); });
// Configuraci�n de almacenamiento de im�genes builder.Services.Configure<ImageSettings>(builder.Configuration.GetSection("ImageSettings")); builder.Services.AddSingleton<IImageService, ImageService>();
// Inyecci�n de dependencias para repositorios builder.Services.AddScoped<IPropiedadRepository, PropiedadRepository>(); // Otros repositorios...
// Inyecci�n de dependencias para servicios builder.Services.AddScoped<IPropiedadService, PropiedadService>(); // Otros servicios...
// Configuraci�n de autenticaci�n JWT builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) .AddJwtBearer(options => { // Configuraci�n del token JWT });


### 5. Ejecuci�n del Proyecto
1. Configure el proyecto API como proyecto de inicio.
2. Presione F5 o utilice `dotnet run` desde la terminal.
3. La API se ejecutar� en `https://localhost:7001` y `http://localhost:5001` por defecto.

## Ejecuci�n de Pruebas Unitarias

### Frameworks y Herramientas Utilizadas
- xUnit: Framework principal de pruebas
- Moq: Biblioteca para simular dependencias
- FluentAssertions: Biblioteca para aserciones m�s expresivas

### Estructura de las Pruebas
Las pruebas est�n organizadas en el proyecto `InmobiliariaMillion.Tests` con la siguiente estrucbiliariaMillion.Tests` con la siguiente estructura:
InmobiliariaMillion.Tests

### C�mo Ejecutar las Pruebas

#### Usando Visual Studio
1. Abra la soluci�n en Visual Studio
2. Abra el Explorador de Pruebas (Prueba > Explorador de pruebas)
3. Haga clic en "Ejecutar todas las pruebas" o seleccione pruebas espec�ficas para ejecutar

#### Usando la L�nea de Comandos
1. Navegue a la carpeta ra�z del proyecto
2. Ejecute el siguiente comando:
 dotnet test

### Verificaci�n de Resultados
- Las pruebas exitosas se marcan en verde
- Las pruebas fallidas se marcan en rojo con detalles del error
- Se genera un informe de cobertura en la carpeta `TestResults`

## Endpoints de API

### Propiedades
- `POST /api/propiedades`: Crear propiedad
- `GET /api/propiedades/{id}`: Obtener propiedad por ID
- `GET /api/propiedades`: Listar propiedades con filtros opcionales
- `PUT /api/propiedades/{id}`: Actualizar propiedad
- `DELETE /api/propiedades/{id}`: Eliminar propiedad

### Gesti�n de Im�genes
- `POST /api/propiedades/{id}/imagenes`: Subir imagen para una propiedad
- `GET /api/propiedades/{id}/imagenes`: Obtener im�genes de una propiedad
- `DELETE /api/propiedades/{id}/imagenes/{imageId}`: Eliminar imagen

## Manejo de CORS
La aplicaci�n est� configurada para permitir solicitudes desde los or�genes especificados en `CorsSettings.AllowedOrigins`. Si necesita a�adir m�s or�genes, actualice el archivo `appsettings.json`.

## Almacenamiento y Procesamiento de Im�genes
- Las im�genes se almacenan en el directorio configurado en `ImageSettings.StoragePath`
- Tama�o m�ximo permitido: 5MB
- Formatos permitidos: JPG, JPEG, PNG
- Las im�genes se procesan para crear miniaturas autom�ticamente

## Verificaci�n de Funcionamiento
1. Acceda a `https://localhost:7001/swagger` para ver la documentaci�n de la API
2. Pruebe los endpoints usando Swagger o cualquier cliente HTTP como Postman
3. Verifique los logs en la consola o en el archivo de log para detectar posibles errores

## Soluci�n de Problemas Comunes
1. **Error de conexi�n a MongoDB**: Verifique que MongoDB est� en ejecuci�n y que la cadena de conexi�n sea correcta.
2. **Problemas con CORS**: Aseg�rese de que el origen del front-end est� incluido en la configuraci�n de CORS.
3. **Error en subida de im�genes**: Compruebe que la carpeta de almacenamiento tenga permisos de escritura.

## Contacto para Soporte
Para cualquier consulta sobre la implementaci�n, contacte a: lopera787@gmail.com

