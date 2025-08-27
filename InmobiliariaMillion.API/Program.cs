using InmobiliariaMillion.Infrastructura;
using Microsoft.Extensions.FileProviders;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InmobiliariaMillion API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Habilita el servicio de archivos est�ticos desde la carpeta wwwroot
app.UseStaticFiles();

app.UseCors("AllowAll");
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
