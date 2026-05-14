using Guia.API.Controllers;
using Guia.API.Data;
using Guia.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System.Text.Json;
using Xunit;

namespace Guia.API.Tests.Controllers
{
    public class PersonasControllerTests
    {
        private Mock<ApplicationDbContext> GetMockContext()
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            return new Mock<ApplicationDbContext>(options) { CallBase = true };
        }

        [Fact]
        public async Task RefreshData_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var personas = new List<Persona>
            {
                new Persona { Id = 1, Nombres = "Test", Apellidos = "User", FechaNacimiento = new DateTime(1990, 1, 1) }
            };
            var frases = new List<FraseGratitud> { new FraseGratitud { Texto = "Gracias", Categoria = "Test" } };

            contextMock.Object.Personas = personas.BuildMockDbSet().Object;
            contextMock.Object.FrasesGratitud = frases.BuildMockDbSet().Object;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.RefreshData(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_Valid_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var personas = new List<Persona>
            {
                new Persona { Username = "user1", Password = "password1", FechaNacimiento = new DateTime(1990, 1, 1) }
            };
            var frases = new List<FraseGratitud> { new FraseGratitud { Texto = "Gracias" } };

            contextMock.Object.Personas = personas.BuildMockDbSet().Object;
            contextMock.Object.FrasesGratitud = frases.BuildMockDbSet().Object;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.Login(new PersonasController.LoginRequest { Username = "user1", Password = "password1" });

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_Invalid_ReturnsUnauthorized()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Personas = new List<Persona>().BuildMockDbSet().Object;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.Login(new PersonasController.LoginRequest { Username = "user1", Password = "wrong" });

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetPersona_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var mockDbSet = new List<Persona>().BuildMockDbSet();
            mockDbSet.Setup(x => x.FindAsync(new object[] { 1 })).ReturnsAsync(new Persona { Id = 1 });
            contextMock.Object.Personas = mockDbSet.Object;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.GetPersona(1);

            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task RecalcularNumerologiaExistente_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var personas = new List<Persona>
            {
                new Persona { Nombres = "John", Apellidos = "Doe", Numerologia = new PersonaNumerologia() }
            };
            contextMock.Object.Personas = personas.BuildMockDbSet().Object;
            contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.RecalcularNumerologiaExistente();

            Assert.Contains("éxito", result);
        }
        
        [Fact]
        public async Task RecalcularTodo_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var personas = new List<Persona>
            {
                new Persona { Nombres = "John", Apellidos = "Doe", Numerologia = new PersonaNumerologia() }
            };
            contextMock.Object.Personas = personas.BuildMockDbSet().Object;
            contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.RecalcularTodo();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task LoginInicial_Valid_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var personas = new List<Persona>
            {
                new Persona { Username = "user1", Password = "password1", FechaNacimiento = new DateTime(1990, 1, 1), Nombres = "Test", Apellidos = "User" }
            };
            var frases = new List<FraseGratitud> { new FraseGratitud { Texto = "Gracias" } };

            contextMock.Object.Personas = personas.BuildMockDbSet().Object;
            contextMock.Object.FrasesGratitud = frases.BuildMockDbSet().Object;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.LoginInicial(new PersonasController.LoginRequest { Username = "user1", Password = "password1" });

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ActualizarPersona_Valid_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var persona = new Persona { Id = 1, Username = "user1", Nombres = "Old", Apellidos = "Old", Numerologia = new PersonaNumerologia(), Detalle = new PersonaDetalle(), ArbolVida = new ArbolVida() };
            var personas = new List<Persona> { persona };
            
            contextMock.Object.Personas = personas.BuildMockDbSet().Object;
            contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var json = @"{ ""nombres"": ""New"", ""apellidos"": ""New"", ""email"": ""new@j.com"", ""fechaNacimiento"": ""1990-01-01"" }";
            var element = JsonDocument.Parse(json).RootElement;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.ActualizarPersona(1, element);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetSignificado_Found_ReturnsOk()
        {
            var contextMock = GetMockContext();
            var temas = new List<Tema> { new Tema { Id = 1, Titulo = "Mision" } };
            var significados = new List<Significado> { new Significado { TemaId = 1, ValorNumero = 5, Apodo = "El Viajero" } };

            contextMock.Object.Temas = temas.BuildMockDbSet().Object;
            contextMock.Object.Significados = significados.BuildMockDbSet().Object;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.GetSignificado("Mision", 5);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RegistrarPersonaAntes_Valid_ReturnsOk()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Personas = new List<Persona>().BuildMockDbSet().Object;
            contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var json = @"{ ""nombres"": ""John"", ""apellidos"": ""Doe"", ""email"": ""j@j.com"", ""username"": ""jdoe"", ""password"": ""pass123"", ""fechaNacimiento"": ""1990-01-01"" }";
            var element = JsonDocument.Parse(json).RootElement;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.RegistrarPersonaL(element);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RegistrarPersona_Valid_ReturnsOk()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Personas = new List<Persona>().BuildMockDbSet().Object;
            contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var json = @"{ ""nombres"": ""John"", ""apellidos"": ""Doe"", ""email"": ""j@j.com"", ""username"": ""jdoe"", ""password"": ""pass123"", ""fechaNacimiento"": ""1990-01-01"" }";
            var element = JsonDocument.Parse(json).RootElement;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.RegistrarPersona(element);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}


