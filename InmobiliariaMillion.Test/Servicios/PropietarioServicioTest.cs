using InmobiliariaMillion.Application.Servicios;
using Moq;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propietario;
using InmobiliariaMillion.Aplicacion.DTOs.Utilidades;
using InmobiliariaMillion.Dominio.Entidades;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InmobiliariaMillion.Test.Servicios
{
    [TestFixture]
    public class PropietarioServicioTest
    {
        private Mock<IPropietarioRepository> _propietarioRepositoryMock = null!;
        private IPropietarioServicio _propietarioService = null!;

        [SetUp]
        public void Setup()
        {
            _propietarioRepositoryMock = new Mock<IPropietarioRepository>();
            _propietarioService = new PropietarioServicio(_propietarioRepositoryMock.Object);
        }

        #region CrearPropietarioAsync Test

        [Test]
        public void CrearAsync_NombreVacio_LanzaExcepcion()
        {
            // Arrange
            var command = new PropietarioInputDto { Nombre = "", Direccion = "Dir", FechaNacimiento = DateTime.Parse("01/01/1980"), Foto = "foto.jpg" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propietarioService.CrearPropietarioAsync(command));
        }

        [Test]
        public void CrearAsync_NombreMuyLargo_LanzaExcepcion()
        {
            // Arrange
            var nombreLargo = new string('A', 101); // 101 caracteres
            var command = new PropietarioInputDto { Nombre = nombreLargo, Direccion = "Dir", FechaNacimiento = DateTime.Parse("01/01/1980"), Foto = "foto.jpg" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propietarioService.CrearPropietarioAsync(command));
        }

        [Test]
        public async Task CrearAsync_CreacionExitosa_RetornaPropietarioDto()
        {
            // Arrange
            var command = new PropietarioInputDto { Nombre = "Nombre", Direccion = "Dir", FechaNacimiento = DateTime.Parse("01/01/1980"), Foto = "foto.jpg" };
            _propietarioRepositoryMock.Setup(repo => repo.CrearAsync(It.IsAny<Propietario>())).ReturnsAsync(new Propietario
            {
                IdPropietario = "nuevo_id",
                Nombre = command.Nombre,
                Direccion = command.Direccion,
                FechaNacimiento = command.FechaNacimiento,
                Foto = command.Foto
            });

            // Act
            var result = await _propietarioService.CrearPropietarioAsync(command);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("nuevo_id", result.IdPropietario);
            Assert.AreEqual(command.Nombre, result.Nombre);
            Assert.AreEqual(command.Direccion, result.Direccion);
            Assert.AreEqual(command.FechaNacimiento.ToString("dd/MM/yyyy"), result.FechaNacimiento);
            Assert.AreEqual(command.Foto, result.Foto);
        }

        #endregion

        #region ObtenerPropietarioPorIdAsync Test

        [Test]
        public async Task ObtenerPropietarioPorIdAsync_IdValido_RetornaPropietarioDto()
        {
            // Arrange
            _propietarioRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(new Propietario
            {
                IdPropietario = "123",
                Nombre = "Nombre",
                Direccion = "Dir",
                FechaNacimiento = DateTime.Parse("01/01/1980"),
                Foto = "foto.jpg"
            });

            // Act
            var result = await _propietarioService.ObtenerPropietarioPorIdAsync("123");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.IdPropietario);
            Assert.AreEqual("Nombre", result.Nombre);
            Assert.AreEqual("Dir", result.Direccion);
            Assert.AreEqual("01/01/1980", result.FechaNacimiento);
            Assert.AreEqual("foto.jpg", result.Foto);
        }

        [Test]
        public async Task ObtenerPropietarioPorIdAsync_IdInvalido_RetornaNull()
        {
            // Arrange
            _propietarioRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync((Propietario?)null);

            // Act
            var result = await _propietarioService.ObtenerPropietarioPorIdAsync("123");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void ObtenerPropietarioPorIdAsync_IdVacio_LanzaExcepcion()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propietarioService.ObtenerPropietarioPorIdAsync(string.Empty));
        }

        #endregion

        #region ObtenerPropietariosAsync Test

        [Test]
        public async Task ObtenerTodosPropietariosAsync_ExistenPropietarios_RetornaPropietariosDto()
        {
            // Arrange
            _propietarioRepositoryMock.Setup(repo => repo.ObtenerAsync(It.IsAny<string?>())).ReturnsAsync(new List<Propietario>
            {
                new Propietario { IdPropietario = "1", Nombre = "Nombre1", Direccion = "Dir1" },
                new Propietario { IdPropietario = "2", Nombre = "Nombre2", Direccion = "Dir2" }
            });

            // Act
            var result = await _propietarioService.ObtenerPropietariosAsync(new FiltrosPropietarioDto());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Nombre1", result[0].Nombre);
            Assert.AreEqual("Nombre2", result[1].Nombre);
        }

        [Test]
        public async Task ObtenerTodosPropietariosAsync_NoExistenPropietarios_RetornaListaVacia()
        {
            // Arrange
            _propietarioRepositoryMock.Setup(repo => repo.ObtenerAsync(It.IsAny<string?>())).ReturnsAsync(new List<Propietario>());

            // Act
            var result = await _propietarioService.ObtenerPropietariosAsync(new FiltrosPropietarioDto());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        #region ObtenerPropietariosPorNombreAsync Test

        [Test]
        public async Task ObtenerPropietariosPorNombreAsync_ExistenPropietarios_RetornaPropietariosDto()
        {
            // Arrange
            string nombre = "Juan";
            _propietarioRepositoryMock.Setup(repo => repo.ObtenerAsync(nombre)).ReturnsAsync(new List<Propietario>
            {
                new Propietario { IdPropietario = "1", Nombre = "Juan Pérez", Direccion = "Dir1" },
                new Propietario { IdPropietario = "2", Nombre = "Juan García", Direccion = "Dir2" }
            });

            // Act
            var filtros = new FiltrosPropietarioDto { Nombre = nombre };
            var result = await _propietarioService.ObtenerPropietariosAsync(filtros);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result[0].Nombre.Contains(nombre));
            Assert.IsTrue(result[1].Nombre.Contains(nombre));
        }

        [Test]
        public async Task ObtenerPropietariosPorNombreAsync_NoExistenPropietarios_RetornaListaVacia()
        {
            // Arrange
            string nombre = "Inexistente";
            _propietarioRepositoryMock.Setup(repo => repo.ObtenerAsync(nombre)).ReturnsAsync(new List<Propietario>());

            // Act
            var filtros = new FiltrosPropietarioDto { Nombre = nombre };
            var result = await _propietarioService.ObtenerPropietariosAsync(filtros);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        #region ActualizarPropietarioAsync Test

        [Test]
        public void ActualizarPropietarioAsync_PropietarioNoExiste_LanzaExcepcion()
        {
            // Arrange
            var command = new PropietarioInputDto { IdPropietario = "123", Nombre = "Nombre", Direccion = "Dir", FechaNacimiento = DateTime.Parse("01/01/1980"), Foto = "foto.jpg" };
            _propietarioRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync((Propietario?)null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propietarioService.ActualizarPropietarioAsync(command));
        }

        [Test]
        public void ActualizarPropietarioAsync_NombreVacio_LanzaExcepcion()
        {
            // Arrange
            var command = new PropietarioInputDto { IdPropietario = "123", Nombre = "", Direccion = "Dir", FechaNacimiento = DateTime.Parse("01/01/1980"), Foto = "foto.jpg" };
            _propietarioRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(new Propietario { IdPropietario = "123" });

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _propietarioService.ActualizarPropietarioAsync(command));
        }

        [Test]
        public async Task ActualizarPropietarioAsync_ActualizacionExitosa_RetornaPropietarioDto()
        {
            // Arrange
            var command = new PropietarioInputDto
            {
                IdPropietario = "123",
                Nombre = "NuevoNombre",
                Direccion = "NuevaDir",
                FechaNacimiento = DateTime.Parse("02/02/1990"),
                Foto = "nuevafoto.jpg"
            };

            _propietarioRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(new Propietario
            {
                IdPropietario = "123",
                Nombre = "Nombre",
                Direccion = "Dir",
                FechaNacimiento = DateTime.Parse("01/01/1980"),
                Foto = "foto.jpg"
            });

            _propietarioRepositoryMock.Setup(repo => repo.ActualizarAsync(It.IsAny<Propietario>())).ReturnsAsync(new Propietario
            {
                IdPropietario = "123",
                Nombre = "NuevoNombre",
                Direccion = "NuevaDir",
                FechaNacimiento = DateTime.Parse("02/02/1990"),
                Foto = "nuevafoto.jpg"
            });

            // Act
            var result = await _propietarioService.ActualizarPropietarioAsync(command);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.IdPropietario);
            Assert.AreEqual("NuevoNombre", result.Nombre);
            Assert.AreEqual("NuevaDir", result.Direccion);
            Assert.AreEqual("02/02/1990", result.FechaNacimiento);
            Assert.AreEqual("nuevafoto.jpg", result.Foto);
        }

        #endregion

        #region EliminarPropietarioAsync Test

        [Test]
        public async Task EliminarPropietarioAsync_PropietarioNoExiste_RetornaFalse()
        {
            // Arrange
            _propietarioRepositoryMock.Setup(repo => repo.ExisteAsync("123")).ReturnsAsync(false);

            // Act
            var result = await _propietarioService.EliminarPropietarioAsync("123");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task EliminarPropietarioAsync_EliminacionExitosa_RetornaTrue()
        {
            // Arrange
            _propietarioRepositoryMock.Setup(repo => repo.ExisteAsync("123")).ReturnsAsync(true);
            _propietarioRepositoryMock.Setup(repo => repo.EliminarAsync("123")).ReturnsAsync(true);

            // Act
            var result = await _propietarioService.EliminarPropietarioAsync("123");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task EliminarPropietarioAsync_EliminacionFallida_RetornaFalse()
        {
            // Arrange
            _propietarioRepositoryMock.Setup(repo => repo.ExisteAsync("123")).ReturnsAsync(true);
            _propietarioRepositoryMock.Setup(repo => repo.EliminarAsync("123")).ReturnsAsync(false);

            // Act
            var result = await _propietarioService.EliminarPropietarioAsync("123");

            // Assert
            Assert.IsFalse(result);
        }

        #endregion
    }
}