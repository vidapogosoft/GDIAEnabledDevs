using Guia.API.Controllers;
using Guia.API.Data;
using Guia.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System.Text.Json;
using Xunit;
using Allure.Xunit.Attributes;

namespace Guia.API.Tests.Controllers
{
    public class PersonasControllerTests_Extra
    {
        private Mock<ApplicationDbContext> GetMockContext()
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            return new Mock<ApplicationDbContext>(options) { CallBase = true };
        }

        [Fact]
        public async Task RegistrarPersona_MissingFields_ReturnsBadRequest()
        {
            var contextMock = GetMockContext();
            var json = @"{ ""nombres"": """", ""apellidos"": """" }";
            var element = JsonDocument.Parse(json).RootElement;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.RegistrarPersona(element);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("obligatorios", badRequest.Value.ToString());
        }

        [Fact]
        public async Task RegistrarPersona_InvalidDate_ReturnsBadRequest()
        {
            var contextMock = GetMockContext();
            var json = @"{ ""nombres"": ""John"", ""apellidos"": ""Doe"", ""email"": ""j@j.com"", ""username"": ""jdoe"", ""password"": ""pass123"", ""fechaNacimiento"": ""invalid"" }";
            var element = JsonDocument.Parse(json).RootElement;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.RegistrarPersona(element);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("fecha", badRequest.Value.ToString());
        }

        [Fact]
        public async Task RegistrarPersona_InvalidUsername_ReturnsBadRequest()
        {
            var contextMock = GetMockContext();
            var json = @"{ ""nombres"": ""John"", ""apellidos"": ""Doe"", ""email"": ""j@j.com"", ""username"": ""jdoe@123"", ""password"": ""pass123"", ""fechaNacimiento"": ""1990-01-01"" }";
            var element = JsonDocument.Parse(json).RootElement;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.RegistrarPersona(element);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("letras y números", badRequest.Value.ToString());
        }

        [Fact]
        public async Task RegistrarPersona_DuplicateUsername_ReturnsBadRequest()
        {
            var contextMock = GetMockContext();
            var personas = new List<Persona> { new Persona { Username = "jdoe" } };
            contextMock.Object.Personas = personas.BuildMockDbSet().Object;

            var json = @"{ ""nombres"": ""John"", ""apellidos"": ""Doe"", ""email"": ""j@j.com"", ""username"": ""jdoe"", ""password"": ""pass123"", ""fechaNacimiento"": ""1990-01-01"" }";
            var element = JsonDocument.Parse(json).RootElement;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.RegistrarPersona(element);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("uso", badRequest.Value.ToString());
        }
        
        [Fact]
        public async Task ActualizarPersona_MissingFields_ReturnsBadRequest()
        {
            var contextMock = GetMockContext();
            var persona = new Persona { Id = 1, Username = "user1", Numerologia = new PersonaNumerologia(), Detalle = new PersonaDetalle(), ArbolVida = new ArbolVida() };
            contextMock.Object.Personas = new List<Persona> { persona }.BuildMockDbSet().Object;

            var json = @"{ ""nombres"": """", ""apellidos"": """" }";
            var element = JsonDocument.Parse(json).RootElement;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.ActualizarPersona(1, element);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async Task ActualizarPersona_NotFound_ReturnsNotFound()
        {
            var contextMock = GetMockContext();
            contextMock.Object.Personas = new List<Persona>().BuildMockDbSet().Object;

            var json = @"{ ""nombres"": ""John"", ""apellidos"": ""Doe"" }";
            var element = JsonDocument.Parse(json).RootElement;

            var controller = new PersonasController(contextMock.Object);
            var result = await controller.ActualizarPersona(1, element);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}


