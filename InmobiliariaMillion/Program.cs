using InmobiliariaMillion.Infrastructura;

var builder = WebApplication.CreateBuilder(args);

// Servicios b�sicos de ASP.NET Core
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar capas de la aplicaci�n
builder.Services.AddInfrastructura(builder.Configuration);
builder.Services.AddApplication();

// Configuraci�n de CORS (opcional)
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

// Middlewares de producci�n
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
