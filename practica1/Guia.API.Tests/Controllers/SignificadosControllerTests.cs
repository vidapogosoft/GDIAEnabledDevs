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
    public class SignificadosControllerTests
    {
        private Mock<ApplicationDbContext> GetMockContext()
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            return new Mock<ApplicationDbContext>(options) { CallBase = true };
        }

        [Fact]
        public async Task GetDetalle_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var significados = new List<Significado>
            {
                new Significado { TemaId = 1, ValorNumero = 5, Tema = new Tema { DescripcionGeneral = "Desc" } }
            };
            contextMock.Object.Significados = significados.BuildMockDbSet().Object;

            var controller = new SignificadosController(contextMock.Object);
            var result = await controller.GetDetalle(1, 5);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetDetalle_NotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Significados = new List<Significado>().BuildMockDbSet().Object;

            var controller = new SignificadosController(contextMock.Object);
            var result = await controller.GetDetalle(1, 5);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}


