using InmobiliariaMillion.Application.Servicios;
using Moq;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad;
using InmobiliariaMillion.Dominio.Entidades;
using NUnit.Framework;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos;

namespace InmobiliariaMillion.Test.Servicios
{
    [TestFixture]
    public class TrazabilidadPropiedadServicioTest
    {
        private Mock<ITrazabilidadPropiedadRepository> _trazabilidadRepositoryMock;
        private Mock<IPropiedadRepository> _propiedadRepositoryMock;
        private ITrazabilidadPropiedadServicio _trazabilidadServicio;
        private IPropiedadServicio _propiedadServicio;
        private IPropietarioServicio _propietarioServicio;

        [SetUp]
        public void Setup()
        {
            _trazabilidadRepositoryMock = new Mock<ITrazabilidadPropiedadRepository>();
            _propiedadRepositoryMock = new Mock<IPropiedadRepository>();
            
            // Crear mock para el servicio de propietario
            var propietarioServicioMock = new Mock<IPropietarioServicio>();
            _propietarioServicio = propietarioServicioMock.Object;
            
            // Ahora podemos inicializar el servicio de propiedad correctamente
            _propiedadServicio = new PropiedadServicio(_propiedadRepositoryMock.Object, _propietarioServicio);
            _trazabilidadServicio = new TrazabilidadPropiedadServicio(_trazabilidadRepositoryMock.Object, _propiedadServicio);
        }

        #region CrearTrazabilidadAsync Test

        [Test]
        public async Task CrearTrazabilidadAsync_PropiedadNoExiste_LanzaExcepcion()
        {
            // Arrange
            var command = new TrazabilidadPropiedadInputDto
            {
                IdPropiedad = "123",
                Valor = 100000,
                FechaVenta = DateTime.Now,
                Nombre = "Venta",
                Impuesto = 15000,
            };
            _propiedadRepositoryMock.Setup(repo => repo.ExisteAsync("123")).ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _trazabilidadServicio.CrearTrazabilidadPropiedadAsync(command));
        }

        [Test]
        public async Task CrearTrazabilidadAsync_PrecioNegativo_LanzaExcepcion()
        {
            // Arrange
            var command = new TrazabilidadPropiedadInputDto
            {
                IdPropiedad = "123",
                Valor = -100000,
                FechaVenta = DateTime.Now,
                Nombre = "Venta",
                Impuesto = 15000,
            };
            _propiedadRepositoryMock.Setup(repo => repo.ExisteAsync("123")).ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _trazabilidadServicio.CrearTrazabilidadPropiedadAsync(command));
        }

        [Test]
        public async Task CrearTrazabilidadAsync_CreacionExitosa_RetornaTrazabilidadDto()
        {
            // Arrange
            var fechaVenta = DateTime.Now.ToString("dd/MM/yyyy");
            var command = new TrazabilidadPropiedadInputDto
            {
                IdPropiedad = "123",
                Valor = 100000,
                FechaVenta = DateTime.Now,
                Nombre = "Venta",
                Impuesto = 15000,
            };

            _propiedadRepositoryMock.Setup(repo => repo.ExisteAsync("123")).ReturnsAsync(true);
            
            // Mock para ObtenerPropiedadPorIdAsync
            var propiedadMock = new PropiedadOutputDto 
            { 
                IdPropiedad = "123", 
                Nombre = "Propiedad de prueba",
                Precio = 100000
            };
            var propiedadServicioMock = new Mock<IPropiedadServicio>();
            propiedadServicioMock.Setup(ps => ps.ObtenerPropiedadPorIdAsync("123")).ReturnsAsync(propiedadMock);
            _propiedadServicio = propiedadServicioMock.Object;
            
            // Recreamos el servicio de trazabilidad con el mock actualizado
            _trazabilidadServicio = new TrazabilidadPropiedadServicio(_trazabilidadRepositoryMock.Object, _propiedadServicio);
            
            _trazabilidadRepositoryMock.Setup(repo => repo.CrearAsync(It.IsAny<TrazabilidadPropiedad>())).ReturnsAsync(
                new TrazabilidadPropiedad
                {
                    IdTrazabilidadPropiedad = "nuevo_id",
                    IdPropiedad = "123",
                    Valor = 100000,
                    FechaVenta = DateTime.Now,
                    Nombre = "Venta",
                    Impuesto = 15000,
                }
            );

            // Act
            var result = await _trazabilidadServicio.CrearTrazabilidadPropiedadAsync(command);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("nuevo_id", result.IdTrazabilidadPropiedad);
            Assert.AreEqual("123", result.IdPropiedad);
            Assert.AreEqual(100000, result.Valor);
        }

        #endregion

        #region ObtenerTrazabilidadPorIdAsync Test

        [Test]
        public async Task ObtenerTrazabilidadPorIdAsync_IdValido_RetornaTrazabilidadDto()
        {
            // Arrange
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(
                new TrazabilidadPropiedad
                {
                    IdTrazabilidadPropiedad = "123",
                    IdPropiedad = "456",
                    Valor = 100000,
                    FechaVenta = DateTime.Now,
                    Nombre = "Venta",
                    Impuesto = 15000,
                }
            );

            // Act
            var result = await _trazabilidadServicio.ObtenerTrazabilidadPropiedadPorIdAsync("123");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.IdTrazabilidadPropiedad);
            Assert.AreEqual("456", result.IdPropiedad);
            Assert.AreEqual(100000, result.Valor);
        }

        [Test]
        public void ObtenerTrazabilidadPorIdAsync_IdVacio_LanzaExcepcion()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _trazabilidadServicio.ObtenerTrazabilidadPropiedadPorIdAsync(string.Empty));
        }

        #endregion

        #region ObtenerTrazabilidadesPorPropiedadAsync Test

        [Test]
        public async Task ObtenerTrazabilidadesPorPropiedadAsync_PropiedadValida_RetornaTrazabilidades()
        {
            // Arrange
            var idPropiedad = "123";
            var fechaVenta1 = DateTime.Now.AddDays(-10);
            var fechaVenta2 = DateTime.Now.AddDays(-5);

            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorPropiedadAsync(idPropiedad)).ReturnsAsync(new List<TrazabilidadPropiedad>
            {
                new TrazabilidadPropiedad {
                    IdTrazabilidadPropiedad = "1",
                    IdPropiedad = idPropiedad,
                    Valor = 100000,
                    FechaVenta = fechaVenta1,
                    Nombre = "Venta 1",
                    Impuesto = 15000
                },
                new TrazabilidadPropiedad {
                    IdTrazabilidadPropiedad = "2",
                    IdPropiedad = idPropiedad,
                    Valor = 120000,
                    FechaVenta = fechaVenta2,
                    Nombre = "Venta 2",
                    Impuesto = 18000
                }
            });

            // Act
            var result = await _trazabilidadServicio.ObtenerPorPropiedadAsync(idPropiedad);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("1", result[0].IdTrazabilidadPropiedad);
            Assert.AreEqual("2", result[1].IdTrazabilidadPropiedad);
            Assert.AreEqual(idPropiedad, result[0].IdPropiedad);
            Assert.AreEqual(idPropiedad, result[1].IdPropiedad);
        }

        [Test]
        public async Task ObtenerTrazabilidadesPorPropiedadAsync_PropiedadSinTrazabilidades_RetornaListaVacia()
        {
            // Arrange
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorPropiedadAsync("123")).ReturnsAsync(new List<TrazabilidadPropiedad>());

            // Act
            var result = await _trazabilidadServicio.ObtenerPorPropiedadAsync("123");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        #region ActualizarTrazabilidadAsync Tests

        [Test]
        public async Task ActualizarTrazabilidadAsync_TrazabilidadNoExiste_LanzaExcepcion()
        {
            // Arrange
            var command = new TrazabilidadPropiedadInputDto
            {
                IdPropiedad = "123",
                Valor = 100000,
                FechaVenta = DateTime.Now,
                Nombre = "Venta",
                Impuesto = 15000,
            };
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync((TrazabilidadPropiedad)null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _trazabilidadServicio.ActualizarTrazabilidadPropiedadAsync(command));
        }

        [Test]
        public async Task ActualizarTrazabilidadAsync_PrecioNegativo_LanzaExcepcion()
        {
            // Arrange
            var command = new TrazabilidadPropiedadInputDto
            {
                IdPropiedad = "123",
                Valor = 100000,
                FechaVenta = DateTime.Now,
                Nombre = "Venta",
                Impuesto = 15000,
            };
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(new TrazabilidadPropiedad { IdTrazabilidadPropiedad = "123" });

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _trazabilidadServicio.ActualizarTrazabilidadPropiedadAsync(command));
        }

        [Test]
        public async Task ActualizarTrazabilidadAsync_ActualizacionExitosa_RetornaTrazabilidadDto()
        {
            // Arrange
            var fechaVenta = DateTime.Now;
            var command = new TrazabilidadPropiedadInputDto
            {
                IdTrazabilidadPropiedad = "123",
                IdPropiedad = "456",
                Valor = 110000,
                FechaVenta = fechaVenta,
                Nombre = "Actualizada",
            };

            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(
                new TrazabilidadPropiedad
                {
                    IdTrazabilidadPropiedad = "123",
                    IdPropiedad = "456",
                    Valor = 100000,
                    FechaVenta = fechaVenta.AddDays(-1),
                }
            );

            _trazabilidadRepositoryMock.Setup(repo => repo.ActualizarAsync(It.IsAny<TrazabilidadPropiedad>())).ReturnsAsync(
                new TrazabilidadPropiedad
                {
                    IdTrazabilidadPropiedad = "123",
                    IdPropiedad = "456",
                    Valor = 110000,
                    FechaVenta = fechaVenta
                }
            );

            // Act
            var result = await _trazabilidadServicio.ActualizarTrazabilidadPropiedadAsync(command);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.IdTrazabilidadPropiedad);
            Assert.AreEqual("456", result.IdPropiedad);
            Assert.AreEqual(110000, result.Valor);
            Assert.AreEqual(fechaVenta, result.FechaVenta);
        }

        #endregion

        #region EliminarTrazabilidadAsync Test

        [Test]
        public async Task EliminarTrazabilidadAsync_TrazabilidadNoExiste_RetornaFalse()
        {
            // Arrange
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync((TrazabilidadPropiedad)null);

            // Act
            var result = await _trazabilidadServicio.EliminarTrazabilidadPropiedadAsync("123");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task EliminarTrazabilidadAsync_EliminacionExitosa_RetornaTrue()
        {
            // Arrange
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(new TrazabilidadPropiedad { IdTrazabilidadPropiedad = "123" });
            _trazabilidadRepositoryMock.Setup(repo => repo.EliminarAsync("123")).ReturnsAsync(true);

            // Act
            var result = await _trazabilidadServicio.EliminarTrazabilidadPropiedadAsync("123");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task EliminarTrazabilidadAsync_EliminacionFallida_RetornaFalse()
        {
            // Arrange
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(new TrazabilidadPropiedad { IdTrazabilidadPropiedad = "123" });
            _trazabilidadRepositoryMock.Setup(repo => repo.EliminarAsync("123")).ReturnsAsync(false);

            // Act
            var result = await _trazabilidadServicio.EliminarTrazabilidadPropiedadAsync("123");

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region ObtenerVentasRecientesAsync Test

        [Test]
        public async Task ObtenerVentasRecientesAsync_ExistenVentas_RetornaVentasRecientes()
        {
            // Arrange
            var fechaInicio = DateTime.Now.AddDays(-30);
            var fechaVenta1 = DateTime.Now.AddDays(-25);
            var fechaVenta2 = DateTime.Now.AddDays(-15);

            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerVentasRecientesAsync(It.IsAny<DateTime>())).ReturnsAsync(new List<TrazabilidadPropiedad>
            {
                new TrazabilidadPropiedad {
                    IdTrazabilidadPropiedad = "1",
                    IdPropiedad = "123",
                    Valor = 90000,
                    FechaVenta = fechaVenta1
                },
                new TrazabilidadPropiedad {
                    IdTrazabilidadPropiedad = "2",
                    IdPropiedad = "456",
                    Valor = 120000,
                    FechaVenta = fechaVenta2
                }
            });

            // Act
            var result = await _trazabilidadServicio.ObtenerVentasRecientesAsync(DateTime.Now.AddDays(-2));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("123", result[0].IdPropiedad);
            Assert.AreEqual("456", result[1].IdPropiedad);
            Assert.AreEqual(90000, result[0].Valor);
        }

        [Test]
        public async Task ObtenerVentasRecientesAsync_NoExistenVentas_RetornaListaVacia()
        {
            // Arrange
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerVentasRecientesAsync(It.IsAny<DateTime>())).ReturnsAsync(new List<TrazabilidadPropiedad>());

            // Act
            var result = await _trazabilidadServicio.ObtenerVentasRecientesAsync(DateTime.Now.AddDays(-2));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #endregion
    }
}