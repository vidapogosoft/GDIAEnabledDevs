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
    public class BitacoraControllerTests
    {
        private Mock<ApplicationDbContext> GetMockContext()
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            return new Mock<ApplicationDbContext>(options) { CallBase = true };
        }

        [Fact]
        public async Task RegistrarEntrada_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>();
            var mockDbSet = bitacoras.BuildMockDbSet();
            // Setup Add to modify list if needed, but not necessary since we just check return
            contextMock.Object.Bitacoras = mockDbSet.Object;
            
            // To simulate SaveChangesAsync
            contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var controller = new BitacoraController(contextMock.Object);
            var result = await controller.RegistrarEntrada(new Bitacora());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetMisRegistros_HasData_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { PersonaId = 1, Fecha = DateTime.Now }
            };
            contextMock.Object.Bitacoras = bitacoras.BuildMockDbSet().Object;

            var controller = new BitacoraController(contextMock.Object);
            var result = await controller.GetMisRegistros(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            // It returns an anonymous object list or empty list
            var val = Assert.IsAssignableFrom<System.Collections.IEnumerable>(okResult.Value);
            Assert.NotEmpty(val);
        }

        [Fact]
        public async Task GetMisRegistros_EmptyData_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>();
            contextMock.Object.Bitacoras = bitacoras.BuildMockDbSet().Object;

            var controller = new BitacoraController(contextMock.Object);
            var result = await controller.GetMisRegistros(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var val = Assert.IsAssignableFrom<System.Collections.IEnumerable>(okResult.Value);
            Assert.Empty(val);
        }

        [Fact]
        public async Task FinalizarReto_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacora = new Bitacora { Id = 1, EstadoReto = "En Curso", Reto = new RetoSemanal() };
            var mockDbSet = new List<Bitacora> { bitacora }.BuildMockDbSet();
            
            // FindAsync needs special setup for MockQueryable or manual
            mockDbSet.Setup(x => x.FindAsync(new object[] { 1 })).ReturnsAsync(bitacora);
            
            contextMock.Object.Bitacoras = mockDbSet.Object;
            contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var controller = new BitacoraController(contextMock.Object);
            var result = await controller.FinalizarReto(1);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Completado", bitacora.EstadoReto);
        }

        [Fact]
        public async Task FinalizarReto_NotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            var mockDbSet = new List<Bitacora>().BuildMockDbSet();
            mockDbSet.Setup(x => x.FindAsync(new object[] { 1 })).ReturnsAsync((Bitacora)null);
            
            contextMock.Object.Bitacoras = mockDbSet.Object;

            var controller = new BitacoraController(contextMock.Object);
            var result = await controller.FinalizarReto(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerHistorial_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { PersonaId = 1, Fecha = DateTime.Now }
            };
            contextMock.Object.Bitacoras = bitacoras.BuildMockDbSet().Object;

            var controller = new BitacoraController(contextMock.Object);
            var result = await controller.ObtenerHistorial(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerRetoActivo_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { PersonaId = 1, EstadoReto = "En Curso", RetoId = 1 }
            };
            var retos = new List<RetoSemanal>
            {
                new RetoSemanal { Id = 1, Titulo = "T", Descripcion = "D", Instrucciones = "I" }
            };
            contextMock.Object.Bitacoras = bitacoras.BuildMockDbSet().Object;
            contextMock.Object.RetosSemanales = retos.BuildMockDbSet().Object;

            var controller = new BitacoraController(contextMock.Object);
            var result = await controller.ObtenerRetoActivo(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ObtenerRetoActivo_NotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Bitacoras = new List<Bitacora>().BuildMockDbSet().Object;
            contextMock.Object.RetosSemanales = new List<RetoSemanal>().BuildMockDbSet().Object;

            var controller = new BitacoraController(contextMock.Object);
            var result = await controller.ObtenerRetoActivo(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}


