using Guia.API.Controllers;
using Guia.API.Data;
using Guia.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Guia.API.Tests.Controllers
{
    public class RetosControllerTests
    {
        private Mock<ApplicationDbContext> GetMockContext()
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            return new Mock<ApplicationDbContext>(options) { CallBase = true };
        }

        [Fact]
        public async Task VerificarRetoActivo_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { PersonaId = 1, EstadoReto = "En Curso", RetoId = 1, Reto = new RetoSemanal { Titulo = "Reto 1", Descripcion = "Desc 1" } }
            };
            contextMock.Object.Bitacoras = bitacoras.BuildMockDbSet().Object;

            var controller = new RetosController(contextMock.Object);
            var result = await controller.VerificarRetoActivo(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task VerificarRetoActivo_NotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Bitacoras = new List<Bitacora>().BuildMockDbSet().Object;

            var controller = new RetosController(contextMock.Object);
            var result = await controller.VerificarRetoActivo(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerEstadoReto_EnCurso_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { PersonaId = 1, EstadoReto = "En Curso", RetoId = 1, Reto = new RetoSemanal { Titulo = "Reto", Descripcion = "Desc" } }
            };
            contextMock.Object.Bitacoras = bitacoras.BuildMockDbSet().Object;
            contextMock.Object.RetosSemanales = new List<RetoSemanal>().BuildMockDbSet().Object;

            var controller = new RetosController(contextMock.Object);
            var result = await controller.ObtenerEstadoReto(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerEstadoReto_LimiteMaximoAlcanzado_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { PersonaId = 1, Tipo = "Reto", EstadoReto = "Completado", Fecha = DateTime.Today }
            };
            contextMock.Object.Bitacoras = bitacoras.BuildMockDbSet().Object;

            var controller = new RetosController(contextMock.Object);
            var result = await controller.ObtenerEstadoReto(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerEstadoReto_NoSentimiento_ReturnsOk_RequiereEscritura()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Bitacoras = new List<Bitacora>().BuildMockDbSet().Object;

            var controller = new RetosController(contextMock.Object);
            var result = await controller.ObtenerEstadoReto(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerEstadoReto_ConSentimiento_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { PersonaId = 1, Tipo = "Sentimiento", Fecha = DateTime.Today, Contenido = "Me siento feliz" }
            };
            var retos = new List<RetoSemanal>
            {
                new RetoSemanal { Id = 1, Activo = true, Titulo = "Reto Silencio", Descripcion = "Silencio", Instrucciones = "Inst" }
            };
            contextMock.Object.Bitacoras = bitacoras.BuildMockDbSet().Object;
            contextMock.Object.RetosSemanales = retos.BuildMockDbSet().Object;

            var controller = new RetosController(contextMock.Object);
            var result = await controller.ObtenerEstadoReto(1);

            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async Task ObtenerEstadoReto_ConSentimiento_NoSugerencia_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { PersonaId = 1, Tipo = "Sentimiento", Fecha = DateTime.Today, Contenido = "Me siento feliz" }
            };
            var retos = new List<RetoSemanal>();
            contextMock.Object.Bitacoras = bitacoras.BuildMockDbSet().Object;
            contextMock.Object.RetosSemanales = retos.BuildMockDbSet().Object;

            var controller = new RetosController(contextMock.Object);
            var result = await controller.ObtenerEstadoReto(1);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}


