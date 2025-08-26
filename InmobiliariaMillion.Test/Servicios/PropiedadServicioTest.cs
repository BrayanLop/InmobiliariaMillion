using InmobiliariaMillion.Application.Servicios;
using Moq;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propiedad;
using InmobiliariaMillion.Aplicacion.DTOs.Utilidades;
using InmobiliariaMillion.Dominio.Entidades;
using NUnit.Framework;

namespace InmobiliariaMillion.Test.Servicios
{
    [TestFixture]
    public class PropiedadServicioTest
    {
        private Mock<IPropiedadRepository> _propiedadRepositoryMock;
        private Mock<IPropietarioRepository> _propietarioRepositoryMock;
        private IPropiedadServicio _propiedadService;

        [SetUp]
        public void Setup()
        {
            _propiedadRepositoryMock = new Mock<IPropiedadRepository>();
            _propietarioRepositoryMock = new Mock<IPropietarioRepository>();
            _propiedadService = new PropiedadServicio(_propiedadRepositoryMock.Object);
        }

        #region CrearPropiedadAsync Tests

        [Test]
        public async Task CrearAsync_PropietarioNoExiste_LanzaExcepcion()
        {
            // Arrange
            var command = new PropiedadInputDto { Nombre = "Casa", Direccion = "Dir", Precio = 100, CodigoInterno = "Cod", Anio = 2024, IdPropietario = "123" };
            _propietarioRepositoryMock.Setup(repo => repo.ExisteAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propiedadService.CrearPropiedadAsync(command));
        }

        [Test]
        public async Task CrearAsync_NombreVacio_LanzaExcepcion()
        {
            // Arrange
            var command = new PropiedadInputDto { Nombre = "", Direccion = "Dir", Precio = 100, CodigoInterno = "Cod", Anio = 2024, IdPropietario = "123" };
            _propietarioRepositoryMock.Setup(repo => repo.ExisteAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propiedadService.CrearPropiedadAsync(command));
        }

        [Test]
        public async Task CrearAsync_PrecioNegativo_LanzaExcepcion()
        {
            // Arrange
            var command = new PropiedadInputDto { Nombre = "Nombre", Direccion = "Dir", Precio = -100, CodigoInterno = "Cod", Anio = 2024, IdPropietario = "123" };
            _propietarioRepositoryMock.Setup(repo => repo.ExisteAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propiedadService.CrearPropiedadAsync(command));
        }

        [Test]
        public async Task CrearAsync_CreacionExitosa_RetornaPropiedadDto()
        {
            // Arrange
            var command = new PropiedadInputDto { Nombre = "Nombre", Direccion = "Dir", Precio = 100, CodigoInterno = "Cod", Anio = 2024, IdPropietario = "123" };
            _propietarioRepositoryMock.Setup(repo => repo.ExisteAsync(It.IsAny<string>())).ReturnsAsync(true);
            _propiedadRepositoryMock.Setup(repo => repo.CrearAsync(It.IsAny<Propiedad>())).ReturnsAsync(new Propiedad { IdPropiedad = "nueva_id", Nombre = command.Nombre });

            // Act
            var result = await _propiedadService.CrearPropiedadAsync(command);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("nueva_id", result.IdPropiedad);
            Assert.AreEqual(command.Nombre, result.Nombre);
        }

        #endregion

        #region ObtenerPropiedadPorIdAsync Tests

        [Test]
        public async Task ObtenerPropiedadPorIdAsync_IdValido_RetornaPropiedadDto()
        {
            // Arrange
            _propiedadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(new Propiedad { IdPropiedad = "123", Nombre = "Nombre" });

            // Act
            var result = await _propiedadService.ObtenerPropiedadPorIdAsync("123");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.IdPropiedad);
            Assert.AreEqual("Nombre", result.Nombre);
        }

        [Test]
        public async Task ObtenerPropiedadPorIdAsync_IdInvalido_RetornaNull()
        {
            // Arrange
            _propiedadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync((Propiedad)null);

            // Act
            var result = await _propiedadService.ObtenerPropiedadPorIdAsync("123");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void ObtenerPropiedadPorIdAsync_IdVacio_LanzaExcepcion()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propiedadService.ObtenerPropiedadPorIdAsync(string.Empty));
        }

        #endregion

        #region ObtenerPropiedadesAsync Tests

        [Test]
        public async Task ObtenerPropiedadesAsync_SinFiltros_RetornaTodasLasPropiedades()
        {
            // Arrange
            _propiedadRepositoryMock.Setup(repo => repo.ObtenerAsync(null, null, null, null)).ReturnsAsync(new List<Propiedad> { new Propiedad { IdPropiedad = "1", Nombre = "Nombre1" }, new Propiedad { IdPropiedad = "2", Nombre = "Nombre2" } });

            // Act
            var result = await _propiedadService.ObtenerPropiedadesAsync(new FiltrosPropiedadDto());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task ObtenerPropiedadesAsync_FiltroPorNombre_RetornaPropiedadesConNombre()
        {
            // Arrange
            _propiedadRepositoryMock.Setup(repo => repo.ObtenerAsync("Nombre1", null, null, null)).ReturnsAsync(new List<Propiedad> { new Propiedad { IdPropiedad = "1", Nombre = "Nombre1" } });

            // Act
            var result = await _propiedadService.ObtenerPropiedadesAsync(new FiltrosPropiedadDto { Nombre = "Nombre1" });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Nombre1", result[0].Nombre);
        }

        [Test]
        public async Task ObtenerPropiedadesAsync_FiltroPorDireccion_RetornaPropiedadesConDireccion()
        {
            // Arrange
            _propiedadRepositoryMock.Setup(repo => repo.ObtenerAsync(null, "Dir1", null, null)).ReturnsAsync(new List<Propiedad> { new Propiedad { IdPropiedad = "1", Direccion = "Dir1" } });

            // Act
            var result = await _propiedadService.ObtenerPropiedadesAsync(new FiltrosPropiedadDto { Direccion = "Dir1" });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Dir1", result[0].Direccion);
        }

        [Test]
        public async Task ObtenerPropiedadesAsync_FiltroPorPrecio_RetornaPropiedadesEnRangoPrecio()
        {
            // Arrange
            _propiedadRepositoryMock.Setup(repo => repo.ObtenerAsync(null, null, 100, 200)).ReturnsAsync(new List<Propiedad> { new Propiedad { IdPropiedad = "1", Precio = 150 } });

            // Act
            var result = await _propiedadService.ObtenerPropiedadesAsync(new FiltrosPropiedadDto { PrecioMinimo = 100, PrecioMaximo = 200 });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(150, result[0].Precio);
        }

        [Test]
        public async Task ObtenerPropiedadesAsync_FiltrosCombinados_RetornaPropiedadesFiltradas()
        {
            // Arrange
            _propiedadRepositoryMock.Setup(repo => repo.ObtenerAsync("Nombre1", "Dir1", 100, 200)).ReturnsAsync(new List<Propiedad> { new Propiedad { IdPropiedad = "1", Nombre = "Nombre1", Direccion = "Dir1", Precio = 150 } });

            // Act
            var result = await _propiedadService.ObtenerPropiedadesAsync(new FiltrosPropiedadDto { Nombre = "Nombre1", Direccion = "Dir1", PrecioMinimo = 100, PrecioMaximo = 200 });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Nombre1", result[0].Nombre);
            Assert.AreEqual("Dir1", result[0].Direccion);
            Assert.AreEqual(150, result[0].Precio);
        }

        #endregion

        #region ActualizarPropiedadAsync Tests

        [Test]
        public async Task ActualizarPropiedadAsync_PropiedadNoExiste_LanzaExcepcion()
        {
            // Arrange
            var command = new PropiedadInputDto { IdPropiedad = "123", Nombre = "Nombre", Direccion = "Dir", Precio = 100, CodigoInterno = "Cod", Anio = 2024, IdPropietario = "123" };
            _propiedadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync((Propiedad)null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propiedadService.ActualizarPropiedadAsync(command));
        }

        [Test]
        public async Task ActualizarPropiedadAsync_ActualizacionExitosa_RetornaPropiedadDto()
        {
            // Arrange
            var command = new PropiedadInputDto { IdPropiedad = "123", Nombre = "NuevoNombre", Direccion = "NuevaDir", Precio = 200, CodigoInterno = "NuevoCod", Anio = 2025, IdPropietario = "456" };
            _propiedadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(new Propiedad { IdPropiedad = "123", Nombre = "Nombre", Direccion = "Dir", Precio = 100, CodigoInterno = "Cod", Anio = 2024, IdPropietario = "456" });
            _propiedadRepositoryMock.Setup(repo => repo.ActualizarAsync(It.IsAny<Propiedad>())).ReturnsAsync(new Propiedad { IdPropiedad = "123", Nombre = "NuevoNombre", Direccion = "NuevaDir", Precio = 200, CodigoInterno = "NuevoCod", Anio = 2025, IdPropietario = "456" });

            // Act
            var result = await _propiedadService.ActualizarPropiedadAsync(command);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.IdPropiedad);
            Assert.AreEqual("NuevoNombre", result.Nombre);
            Assert.AreEqual(200, result.Precio);
        }

        #endregion

        #region EliminarPropiedadAsync Tests

        [Test]
        public async Task EliminarPropiedadAsync_EliminacionExitosa_RetornaTrue()
        {
            // Arrange
            _propiedadRepositoryMock.Setup(repo => repo.EliminarAsync("123")).ReturnsAsync(true);

            // Act
            var result = await _propiedadService.EliminarPropiedadAsync("123");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task EliminarPropiedadAsync_EliminacionFallida_RetornaFalse()
        {
            // Arrange
            _propiedadRepositoryMock.Setup(repo => repo.EliminarAsync("123")).ReturnsAsync(false);

            // Act
            var result = await _propiedadService.EliminarPropiedadAsync("123");

            // Assert
            Assert.IsFalse(result);
        }

        #endregion
    }
}