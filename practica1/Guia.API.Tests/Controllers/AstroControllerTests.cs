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
    public class AstroControllerTests
    {
        private Mock<ApplicationDbContext> GetMockContext()
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var contextMock = new Mock<ApplicationDbContext>(options) { CallBase = true };
            return contextMock;
        }

        [Fact]
        public async Task GetDetalle_EmptyName_ReturnsBadRequest()
        {
            var contextMock = GetMockContext();
            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetDetalle("signo", "");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetDetalle_Signo_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var signos = new List<SignoZodiacal> { new SignoZodiacal { Nombre = "Aries" } };
            contextMock.Object.SignosZodiacales = signos.BuildMockDbSet().Object;

            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetDetalle("signo", "Aries");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetDetalle_SignoNotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.SignosZodiacales = new List<SignoZodiacal>().BuildMockDbSet().Object;

            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetDetalle("signo", "Aries");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetDetalle_Luna_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var lunas = new List<FaseLunar> { new FaseLunar { Nombre = "Llena" } };
            contextMock.Object.FasesLunares = lunas.BuildMockDbSet().Object;

            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetDetalle("luna", "Llena");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetDetalle_LunaNotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.FasesLunares = new List<FaseLunar>().BuildMockDbSet().Object;

            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetDetalle("luna", "Llena");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetDetalle_Elemento_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var elementos = new List<ElementoAstro> { new ElementoAstro { Nombre = "Fuego" } };
            contextMock.Object.ElementosAstro = elementos.BuildMockDbSet().Object;

            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetDetalle("elemento", "Fuego");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetDetalle_ElementoNotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.ElementosAstro = new List<ElementoAstro>().BuildMockDbSet().Object;

            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetDetalle("elemento", "Fuego");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetDetalle_InvalidTipo_ReturnsBadRequest()
        {
            var contextMock = GetMockContext();
            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetDetalle("invalido", "Fuego");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetSigno_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var signos = new List<SignoZodiacal> { new SignoZodiacal { Nombre = "Aries" } };
            contextMock.Object.SignosZodiacales = signos.BuildMockDbSet().Object;

            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetSigno("Aries");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetSigno_NotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.SignosZodiacales = new List<SignoZodiacal>().BuildMockDbSet().Object;

            var controller = new AstroController(contextMock.Object);
            var result = await controller.GetSigno("Aries");

            Assert.IsType<NotFoundResult>(result);
        }
    }
}


