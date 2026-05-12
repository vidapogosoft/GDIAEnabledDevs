# MoqDemo — Uso de Moq en C# .NET 8

> **Moq** es el equivalente de **Mockito** en el ecosistema .NET.

## Estructura del proyecto

```
MoqDemo/
├── src/
│   └── MoqDemo.Core/
│       ├── Models/          → Product, Order
│       ├── Interfaces/      → IProductRepository, IOrderRepository, IEmailService, IAppLogger
│       ├── Exceptions/      → DomainException
│       └── Services/        → OrderService  ← clase bajo prueba
└── tests/
    └── MoqDemo.Tests/
        └── OrderServiceTests.cs  ← todos los ejemplos de Moq aquí
```

## Equivalencias Mockito ↔ Moq

| Mockito (Java)                                              | Moq (C#)                                                   |
|-------------------------------------------------------------|------------------------------------------------------------|
| `@Mock`                                                     | `new Mock<IProductRepository>()`                           |
| `@InjectMocks`                                              | Inyección manual en constructor                            |
| `when(repo.getById(1)).thenReturn(product)`                 | `Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product)`      |
| `when(repo.save(any())).thenThrow(new RuntimeException())` | `Setup(...).ThrowsAsync(new Exception())`                  |
| `verify(email).send("x@x.com", 1)`                         | `Verify(e => e.SendAsync("x@x.com", 1), Times.Once())`     |
| `verify(email, never()).send(...)`                          | `Verify(e => e.SendAsync(...), Times.Never())`             |
| `verify(email, times(3)).send(...)`                         | `Verify(e => e.SendAsync(...), Times.Exactly(3))`          |
| `ArgumentCaptor<Order>`                                     | `.Callback<Order>(o => capturedOrder = o)`                 |
| `any()` / `anyInt()`                                        | `It.IsAny<Order>()` / `It.IsAny<int>()`                   |
| `eq(5)` / argThat(x -> x > 0)                              | `It.Is<int>(x => x == 5)` / `It.Is<int>(x => x > 0)`      |
| `verifyNoInteractions(repo)`                                | `mock.VerifyNoOtherCalls()`                                |
| `MockBehavior.STRICT`                                       | `new Mock<T>(MockBehavior.Strict)`                         |

## Conceptos demostrados en los tests

| # | Concepto                        | Test                                                         |
|---|---------------------------------|--------------------------------------------------------------|
| 1 | Configurar retornos async       | `Setup().ReturnsAsync()`                                     |
| 2 | Verificar que se llamó algo     | `Verify(..., Times.Once())`                                  |
| 3 | Verificar que NO se llamó algo  | `Verify(..., Times.Never())`                                 |
| 4 | Lanzar excepciones desde mocks  | `Setup().ThrowsAsync()`                                      |
| 5 | Capturar argumentos (Callback)  | `Setup().Callback<T>(x => captured = x)`                    |
| 6 | Matchers condicionales          | `It.Is<T>(x => condición)`                                   |
| 7 | Número exacto de llamadas       | `Times.Exactly(n)`                                           |
| 8 | Sin otras interacciones         | `VerifyNoOtherCalls()`                                       |

## Cómo ejecutar

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar todos los tests
dotnet test

# Ejecutar con detalle
dotnet test --logger "console;verbosity=detailed"

# Solo un test por nombre
dotnet test --filter "DisplayName~Callback"
```

## Paquetes NuGet utilizados

- **Moq 4.20** — framework de mocking
- **xunit 2.7** — framework de testing
- **FluentAssertions 6.12** — asserts más legibles
