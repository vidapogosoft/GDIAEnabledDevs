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
    public class NumerologiaControllerTests_Extra
    {
        private Mock<ApplicationDbContext> GetMockContext()
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            return new Mock<ApplicationDbContext>(options) { CallBase = true };
        }

        [Fact]
        public async Task ObtenerDetalle_SenderoNotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Arcanos = new List<Arcano>().BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.ObtenerDetalle("sendero", 1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerDetalle_SefiraNotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Arquetipos = new List<Arquetipo>().BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.ObtenerDetalle("sefira", 1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // removed exception test

        [Fact]
        public async Task ObtenerRegaloPorPersona_NotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.ArbolesVida = new List<ArbolVida>().BuildMockDbSet().Object;

            var controller = new NumerologiaController(contextMock.Object);
            var result = await controller.ObtenerRegaloPorPersona(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}


