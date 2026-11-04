using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SolicitudesApp.Domain.Excepciones;

namespace SolicitudesApp.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) { 
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex) {
            _logger.LogError(ex, "Ocurrió una excepcion no controlada: {Message}", ex.Message);

            var (statusCode, problemDetails) = ex switch
            {
                ValidationException valEx => (
                    HttpStatusCode.BadRequest,
                    new HttpValidationProblemDetails(
                        valEx.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                        )
                ),
                ReglaDominioException domEx => (
                    HttpStatusCode.UnprocessableEntity,
                    new ProblemDetails
                    {
                        Title = "Violación de regla de Negocio Corporativa",
                        Status = (int)HttpStatusCode.UnprocessableEntity,
                        Detail = domEx.Message,
                        Instance = context.Request.Path
                    }
                ),
                KeyNotFoundException notFoundEx => (
                    HttpStatusCode.NotFound,
                    new ProblemDetails
                    {
                        Title = "Recurso no encontrado",
                        Status = (int)HttpStatusCode.NotFound,
                        Detail = notFoundEx.Message,
                        Instance = context.Request.Path
                    }
                ),
                _ => (
                    HttpStatusCode.InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Error interno del servidor",
                        Status = (int)HttpStatusCode.InternalServerError,
                        Detail = "Ha ocurrido un error inesperado al procesar la solicitud.",
                        Instance = context.Request.Path
                    }
                )
            };

            context.Response.ContentType ="application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize((object)problemDetails, new JsonSerializerOptions { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }

    }
}
