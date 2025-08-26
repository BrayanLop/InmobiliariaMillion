using InmobiliariaMillion.Application.Servicios;
using Moq;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad;
using InmobiliariaMillion.Dominio.Entidades;
using NUnit.Framework;

namespace InmobiliariaMillion.Test.Servicios
{
    [TestFixture]
    public class TrazabilidadPropiedadServicioTest
    {
        private Mock<ITrazabilidadPropiedadRepository> _trazabilidadRepositoryMock;
        private Mock<IPropiedadRepository> _propiedadRepositoryMock;
        private ITrazabilidadPropiedadServicio _trazabilidadServicio;

        [SetUp]
        public void Setup()
        {
            _trazabilidadRepositoryMock = new Mock<ITrazabilidadPropiedadRepository>();
            _propiedadRepositoryMock = new Mock<IPropiedadRepository>();
            _trazabilidadServicio = new TrazabilidadPropiedadServicio(_trazabilidadRepositoryMock.Object);
        }

        #region CrearTrazabilidadAsync Tests
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
                Valor = 100000,
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
            _trazabilidadRepositoryMock.Setup(repo => repo.CrearAsync(It.IsAny<TrazabilidadPropiedad>())).ReturnsAsync(
                new TrazabilidadPropiedad
                {
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

        #region ObtenerTrazabilidadPorIdAsync Tests
        [Test]
        public async Task ObtenerTrazabilidadPorIdAsync_IdValido_RetornaTrazabilidadDto()
        {
            // Arrange
            var fechaVenta = DateTime.Now.Date;
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(
                new TrazabilidadPropiedad
                {
                    IdPropiedad = "123",
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
            Assert.AreEqual(fechaVenta.ToString("dd/MM/yyyy"), result.FechaVenta);
        }

        [Test]
        public async Task ObtenerTrazabilidadPorIdAsync_IdInvalido_RetornaNull()
        {
            // Arrange
            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync((TrazabilidadPropiedad)null);

            // Act
            var result = await _trazabilidadServicio.ObtenerTrazabilidadPropiedadPorIdAsync("123");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void ObtenerTrazabilidadPorIdAsync_IdVacio_LanzaExcepcion()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _trazabilidadServicio.ObtenerTrazabilidadPropiedadPorIdAsync(string.Empty));
        }
        #endregion

        #region ObtenerTrazabilidadesPorPropiedadAsync Tests
        [Test]
        public async Task ObtenerTrazabilidadesPorPropiedadAsync_PropiedadValida_RetornaTrazabilidades()
        {
            // Arrange
            var fechaVenta1 = DateTime.Now.AddDays(-10);
            var fechaVenta2 = DateTime.Now.AddDays(-5);

            _trazabilidadRepositoryMock.Setup(repo => repo.ObtenerPorPropiedadAsync("123")).ReturnsAsync(new List<TrazabilidadPropiedad>
            {
                new TrazabilidadPropiedad {
                    IdPropiedad = "1",
                    Valor = 100000,
                    FechaVenta = DateTime.Now,
                    Nombre = "Venta",
                    Impuesto = 15000
                },
                new TrazabilidadPropiedad {
                    IdPropiedad = "2",
                    Valor = 100000,
                    FechaVenta = DateTime.Now,
                    Nombre = "Venta",
                    Impuesto = 15000
                }
            });

            // Act
            var result = await _trazabilidadServicio.ObtenerTrazabilidadPropiedadPorIdAsync("123");

            // Assert
            Assert.IsNotNull(result);
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

        [Test]
        public void ObtenerTrazabilidadesPorPropiedadAsync_IdPropiedadVacio_LanzaExcepcion()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _trazabilidadServicio.ObtenerPorPropiedadAsync(string.Empty));
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

        #region EliminarTrazabilidadAsync Tests
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

        #region ObtenerVentasRecientesAsync Tests
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

        [Test]
        public void ObtenerVentasRecientesAsync_DiasNegativos_LanzaExcepcion()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _trazabilidadServicio.ObtenerVentasRecientesAsync(DateTime.Now.AddDays(-2)));
        }
        #endregion
    }
}