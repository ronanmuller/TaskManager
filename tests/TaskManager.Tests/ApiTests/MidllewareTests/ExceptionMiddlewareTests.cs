using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace TaskManager.Api.Middleware
{
    public class ExceptionMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly Mock<ILogger<ExceptionMiddleware>> _loggerMock;
        private readonly Mock<IHostEnvironment> _hostEnvironmentMock;
        private readonly ExceptionMiddleware _middleware;

        public ExceptionMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
            _hostEnvironmentMock = new Mock<IHostEnvironment>();
            _middleware = new ExceptionMiddleware(_nextMock.Object, _loggerMock.Object, _hostEnvironmentMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_ShouldLogAndReturnBadRequest_WhenArgumentNullExceptionOccurs()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var exception = new ArgumentNullException("testArg");
            _nextMock.Setup(next => next(context)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(logger => logger.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Argumento nulo: testArg")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldLogAndReturnBadRequest_WhenArgumentExceptionOccurs()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var exception = new ArgumentException("Argumento inválido.");
            _nextMock.Setup(next => next(context)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(logger => logger.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Argumento inválido: Argumento inválido.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldLogAndReturnNotFound_WhenKeyNotFoundExceptionOccurs()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var exception = new KeyNotFoundException("Dado não encontrado.");
            _nextMock.Setup(next => next(context)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(logger => logger.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Dado não encontrado: Dado não encontrado.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldLogAndReturnForbidden_WhenUnauthorizedAccessExceptionOccurs()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var exception = new UnauthorizedAccessException();
            _nextMock.Setup(next => next(context)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(logger => logger.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Acesso não autorizado.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldLogAndReturnForbidden_WhenInvalidOperationExceptionOccurs()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var exception = new InvalidOperationException();
            _nextMock.Setup(next => next(context)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(logger => logger.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Operação inválida.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldLogAndReturnInternalServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var exception = new Exception("Erro não tratado.");
            _nextMock.Setup(next => next(context)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(logger => logger.Log(LogLevel.Error, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro não tratado: Erro não tratado.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

        private async Task<string> GetResponseBody(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body);
            return await reader.ReadToEndAsync();
        }
    }
}
