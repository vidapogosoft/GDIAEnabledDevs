using Guia.API.Controllers;
using Guia.API.Data;
using Guia.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;
using Allure.Xunit.Attributes;

namespace Guia.API.Tests.Controllers
{
    public class NumerologiaControllerTests
    {
        private Mock<ApplicationDbContext> GetMockContext()
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            return new Mock<ApplicationDbContext>(options) { CallBase = true };
        }

        [Fact]
        public async Task GetSignificado_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var significados = new List<Significado>
            {
                new Significado { TemaId = 1, ValorNumero = 1, Apodo = "Test" }
            };
            contextMock.Object.Significados = significados.BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.GetSignificado(1, 1);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetSignificado_NotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Significados = new List<Significado>().BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.GetSignificado(1, 1);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetTemas_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var temas = new List<Tema>
            {
                new Tema { EstaActivo = true }
            };
            contextMock.Object.Temas = temas.BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.GetTemas();

            var list = Assert.IsAssignableFrom<IEnumerable<Tema>>(result.Value);
            Assert.Single(list);
        }

        [Fact]
        public async Task CalcularArbolDeLaVida_InvalidRequest_ReturnsBadRequest()
        {
            var contextMock = GetMockContext();
            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.CalcularArbolDeLaVida(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CalcularArbolDeLaVida_ValidRequest_ReturnsOk()
        {
            var contextMock = GetMockContext();
            contextMock.Object.ArbolesVida = new List<ArbolVida>().BuildMockDbSet().Object;
            contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var controller = new NumerologiaController(contextMock.Object);
            var req = new ArbolVidaRequest 
            {
                Persona = new Persona { Id = 1, FechaNacimiento = new DateTime(1990, 1, 1) },
                Datos = new DatosNumerologiaDto { MisionVida = 1, NumeroAlma = 1, NumeroPersonalidad = 1, RegaloDivino = 1 }
            };

            var result = await controller.CalcularArbolDeLaVida(req);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerArbol_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var arboles = new List<ArbolVida> { new ArbolVida { PersonaId = 1 } };
            contextMock.Object.ArbolesVida = arboles.BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.ObtenerArbol(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerArbol_NotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.ArbolesVida = new List<ArbolVida>().BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.ObtenerArbol(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerDetalle_Sendero_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var arcanos = new List<Arcano> { new Arcano { Numero = 1, Nombre = "Mago" } };
            contextMock.Object.Arcanos = arcanos.BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.ObtenerDetalle("sendero", 1, "");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerDetalle_Sefira_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var arquetipos = new List<Arquetipo> { new Arquetipo { Numero = 1, Nombre = "Kether" } };
            contextMock.Object.Arquetipos = arquetipos.BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.ObtenerDetalle("sefira", 1, "Kether");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerRegaloPorPersona_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var arboles = new List<ArbolVida> { new ArbolVida { PersonaId = 1, Kether_Valor = 5 } };
            contextMock.Object.ArbolesVida = arboles.BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.ObtenerRegaloPorPersona(1);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}


