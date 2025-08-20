using InmobiliariaMillion.Infrastructura;

var builder = WebApplication.CreateBuilder(args);

// Servicios básicos de ASP.NET Core
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar capas de la aplicación
builder.Services.AddInfrastructura(builder.Configuration);
builder.Services.AddApplication();

// Configuración de CORS (opcional)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InmobiliariaMillion API V1");
        c.RoutePrefix = string.Empty;
    });
}

// Middlewares de producción
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
