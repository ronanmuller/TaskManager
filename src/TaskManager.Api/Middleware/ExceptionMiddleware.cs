using System.Net;
using Newtonsoft.Json;

namespace TaskManager.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Log a generic error message
            _logger.LogError(ex, "Foi gerado um erro na requisição.");

            // Log specific error messages based on the exception type
            switch (ex)
            {
                case ArgumentNullException argEx:
                    _logger.LogWarning("Argumento nulo: {ArgumentName}", argEx.ParamName);
                    break;
                case ArgumentException argEx:
                    _logger.LogWarning("Argumento inválido: {Message}", argEx.Message);
                    break;
                case KeyNotFoundException keyEx:
                    _logger.LogWarning("Dado não encontrado: {Message}", keyEx.Message);
                    break;
                case UnauthorizedAccessException _:
                    _logger.LogWarning("Acesso não autorizado.");
                    break;
                case InvalidOperationException _:
                    _logger.LogWarning("Operação inválida.");
                    break;
                default:
                    _logger.LogError("Erro não tratado: {Message}", ex.Message);
                    break;
            }

            // Set response content type
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            switch (ex)
            {
                case ArgumentNullException:
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound; // 404
                    break;
                case UnauthorizedAccessException:
                case InvalidOperationException:
                    statusCode = HttpStatusCode.Forbidden; // 403
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = (int)statusCode,
                Message = ex.Message,
                StackTrace = _hostEnvironment.IsDevelopment() ? ex.StackTrace : null
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

    }
}
