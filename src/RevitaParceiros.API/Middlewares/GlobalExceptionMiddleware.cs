using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Domain.Exceptions;

namespace RevitaParceiros.API.Middlewares;

/// <summary>
/// Middleware global de tratamento de exceções.
/// Captura exceções e retorna respostas padronizadas (ProblemDetails).
/// Nunca expõe stack traces ou detalhes internos em produção.
/// </summary>
public sealed class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IHostEnvironment environment)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            NotFoundException => ((int)HttpStatusCode.NotFound, "Recurso não encontrado"),
            BusinessRuleException => ((int)HttpStatusCode.UnprocessableEntity, "Violação de regra de negócio"),
            UnauthorizedException => ((int)HttpStatusCode.Forbidden, "Acesso negado"),
            ValidationException => ((int)HttpStatusCode.BadRequest, "Erro de validação"),
            _ => ((int)HttpStatusCode.InternalServerError, "Erro interno do servidor")
        };

        logger.LogError(exception, "Exceção capturada: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception is ValidationException validationEx
                ? string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage))
                : exception.Message,
            Instance = context.Request.Path
        };

        // Em produção, não expor detalhes de exceções internas
        if (statusCode == (int)HttpStatusCode.InternalServerError && !environment.IsDevelopment())
        {
            problemDetails.Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
        }

        // Em desenvolvimento, incluir stack trace para exceções internas
        if (environment.IsDevelopment() && statusCode == (int)HttpStatusCode.InternalServerError)
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
        }

        // Adicionar erros de validação detalhados
        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails, JsonOptions));
    }
}
