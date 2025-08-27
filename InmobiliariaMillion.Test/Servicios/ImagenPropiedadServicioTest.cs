using InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad;
using InmobiliariaMillion.Aplicacion.Servicios;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Application.Servicios;
using InmobiliariaMillion.Dominio.Entidades;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;

namespace InmobiliariaMillion.Test.Servicios
{
    [TestFixture]
    public class ImagenPropiedadServicioTest
    {
        private Mock<IImagenPropiedadRepository> _imagenRepositoryMock;
        private Mock<IPropiedadRepository> _propiedadRepositoryMock;
        private IImagenPropiedadServicio _imagenServicio;

        [SetUp]
        public void Setup()
        {
            _imagenRepositoryMock = new Mock<IImagenPropiedadRepository>();
            _propiedadRepositoryMock = new Mock<IPropiedadRepository>();
            _imagenServicio = new ImagenPropiedadServicio(_propiedadRepositoryMock.Object, _imagenRepositoryMock.Object);
        }

        #region AgregarImagenAPropiedadAsync Test

        [Test]
        public async Task AgregarImagenAPropiedadAsync_PropiedadNoExiste_LanzaExcepcion()
        {
            // Arrange
            var command = new ImagenPropiedadInputDto
            {
                IdPropiedad = "123",
                Archivo = "imagen.jpg",
                Habilitada = true
            };
            _propiedadRepositoryMock.Setup(repo => repo.ExisteAsync("123")).ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _imagenServicio.AgregarImagenAPropiedadAsync(command));
        }

        [Test]
        public async Task AgregarImagenAPropiedadAsync_CreacionExitosa_RetornaImagenDto()
        {
            // Arrange
            var command = new ImagenPropiedadInputDto
            {
                IdPropiedad = "123",
                Archivo = "imagen.jpg",
                Habilitada = true
            };

            _propiedadRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync("123")).ReturnsAsync(
                new Propiedad { IdPropiedad = "123", Nombre = "Propiedad Test" }
            );
            _imagenRepositoryMock.Setup(repo => repo.CrearAsync(It.IsAny<ImagenPropiedad>())).ReturnsAsync(
                new ImagenPropiedad
                {
                    IdImagenPropiedad = "img_123",
                    IdPropiedad = "123",
                    Archivo = "imagen.jpg",
                    Habilitada = true
                }
            );

            // Act
            var result = await _imagenServicio.AgregarImagenAPropiedadAsync(command);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("img_123", result.IdImagenPropiedad);
            Assert.AreEqual("123", result.IdPropiedad);
            Assert.AreEqual("imagen.jpg", result.Archivo);
            Assert.IsTrue(result.Habilitada);
        }

        #endregion

        #region ObtenerImagenesPorPropiedadAsync Test

        [Test]
        public async Task ObtenerImagenesPorPropiedadAsync_PropiedadValida_RetornaImagenes()
        {
            // Arrange
            var propiedadId = "123";
            _imagenRepositoryMock.Setup(repo => repo.ObtenerPorPropiedadAsync(propiedadId)).ReturnsAsync(new List<ImagenPropiedad>
            {
                new ImagenPropiedad {
                    IdImagenPropiedad = "img1",
                    IdPropiedad = propiedadId,
                    Archivo = "imagen1.jpg",
                    Habilitada = true
                },
                new ImagenPropiedad {
                    IdImagenPropiedad = "img2",
                    IdPropiedad = propiedadId,
                    Archivo = "imagen2.jpg",
                    Habilitada = true
                }
            });

            // Act
            var result = await _imagenServicio.ObtenerImagenesPorPropiedadAsync(propiedadId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("img1", result[0].IdImagenPropiedad);
            Assert.AreEqual("img2", result[1].IdImagenPropiedad);
        }

        [Test]
        public async Task ObtenerImagenesPorPropiedadAsync_PropiedadSinImagenes_RetornaListaVacia()
        {
            // Arrange
            var propiedadId = "123";
            _imagenRepositoryMock.Setup(repo => repo.ObtenerPorPropiedadAsync(propiedadId)).ReturnsAsync(new List<ImagenPropiedad>());

            // Act
            var result = await _imagenServicio.ObtenerImagenesPorPropiedadAsync(propiedadId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        #region ObtenerPropiedadImagenAsync Test

        [Test]
        public async Task ObtenerPropiedadImagenAsync_ExistenImagenes_RetornaTodasLasImagenes()
        {
            // Arrange
            _imagenRepositoryMock.Setup(repo => repo.ObtenerPorPropiedadAsync(null)).ReturnsAsync(new List<ImagenPropiedad>
            {
                new ImagenPropiedad {
                    IdImagenPropiedad = "img1",
                    IdPropiedad = "123",
                    Archivo = "imagen1.jpg",
                    Habilitada = true
                },
                new ImagenPropiedad {
                    IdImagenPropiedad = "img2",
                    IdPropiedad = "456",
                    Archivo = "imagen2.jpg",
                    Habilitada = true
                }
            });

            // Act
            var result = await _imagenServicio.ObtenerPropiedadImagenAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("img1", result[0].IdImagenPropiedad);
            Assert.AreEqual("img2", result[1].IdImagenPropiedad);
        }

        [Test]
        public async Task ObtenerPropiedadImagenAsync_NoExistenImagenes_RetornaListaVacia()
        {
            // Arrange
            _imagenRepositoryMock.Setup(repo => repo.ObtenerPorPropiedadAsync(null)).ReturnsAsync(new List<ImagenPropiedad>());

            // Act
            var result = await _imagenServicio.ObtenerPropiedadImagenAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        #region EliminarImagenAsync Test

        [Test]
        public async Task EliminarImagenAsync_ImagenNoExiste_RetornaFalso()
        {
            // Arrange
            var imagenId = "img1";
            _imagenRepositoryMock.Setup(repo => repo.ExisteAsync(imagenId)).ReturnsAsync(false);

            // Act
            var result = await _imagenServicio.EliminarImagenAsync(imagenId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task EliminarImagenAsync_EliminacionExitosa_RetornaTrue()
        {
            // Arrange
            var imagenId = "img1";
            _imagenRepositoryMock.Setup(repo => repo.ExisteAsync(imagenId)).ReturnsAsync(true);
            _imagenRepositoryMock.Setup(repo => repo.EliminarAsync(imagenId)).ReturnsAsync(true);

            // Act
            var result = await _imagenServicio.EliminarImagenAsync(imagenId);

            // Assert
            Assert.IsTrue(result);
            _imagenRepositoryMock.Verify(repo => repo.EliminarAsync(imagenId), Times.Once);
        }

        #endregion

        #region EliminarPropiedadAsync Tests
        [Test]
        public async Task EliminarPropiedadAsync_PropiedadTieneImagenes_EliminaTodas()
        {
            // Arrange
            var propiedadId = "123";
            _imagenRepositoryMock.Setup(repo => repo.ObtenerPorPropiedadAsync(propiedadId)).ReturnsAsync(new List<ImagenPropiedad>
            {
                new ImagenPropiedad { IdImagenPropiedad = "img1", IdPropiedad = propiedadId },
                new ImagenPropiedad { IdImagenPropiedad = "img2", IdPropiedad = propiedadId }
            });
            
            _imagenRepositoryMock.Setup(repo => repo.EliminarAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _imagenServicio.EliminarPropiedadAsync(propiedadId);

            // Assert
            Assert.IsTrue(result);
            _imagenRepositoryMock.Verify(repo => repo.EliminarAsync("img1"), Times.Once);
            _imagenRepositoryMock.Verify(repo => repo.EliminarAsync("img2"), Times.Once);
        }

        [Test]
        public async Task EliminarPropiedadAsync_PropiedadSinImagenes_RetornaTrue()
        {
            // Arrange
            var propiedadId = "123";
            _imagenRepositoryMock.Setup(repo => repo.ObtenerPorPropiedadAsync(propiedadId)).ReturnsAsync(new List<ImagenPropiedad>());

            // Act
            var result = await _imagenServicio.EliminarPropiedadAsync(propiedadId);

            // Assert
            Assert.IsTrue(result);
            _imagenRepositoryMock.Verify(repo => repo.EliminarAsync(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region HabilitarImagenAsync Tests
        [Test]
        public async Task HabilitarImagenAsync_ImagenNoExiste_RetornaFalso()
        {
            // Arrange
            var imagenId = "img1";
            _imagenRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync(imagenId)).ReturnsAsync((ImagenPropiedad)null);

            // Act
            var result = await _imagenServicio.HabilitarImagenAsync(imagenId, true);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task HabilitarImagenAsync_ActualizacionExitosa_RetornaTrue()
        {
            // Arrange
            var imagenId = "img1";
            var imagen = new ImagenPropiedad
            {
                IdImagenPropiedad = imagenId,
                IdPropiedad = "123",
                Archivo = "imagen.jpg",
                Habilitada = false
            };

            _imagenRepositoryMock.Setup(repo => repo.ObtenerPorIdAsync(imagenId)).ReturnsAsync(imagen);
            _imagenRepositoryMock.Setup(repo => repo.ActualizarAsync(It.IsAny<ImagenPropiedad>())).ReturnsAsync(
                (ImagenPropiedad img) => { img.Habilitada = true; return img; }
            );

            // Act
            var result = await _imagenServicio.HabilitarImagenAsync(imagenId, true);

            // Assert
            Assert.IsTrue(result);
            _imagenRepositoryMock.Verify(repo => repo.ActualizarAsync(It.Is<ImagenPropiedad>(i => i.IdImagenPropiedad == imagenId && i.Habilitada == true)), Times.Once);
        }

        #endregion
    }
}
