using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace RevitaParceiros.Infra.Persistence.Converters;

/// <summary>
/// Converter global para garantir que todo DateTime vindo da aplicação seja tratado como Horário de Brasília
/// antes de ser convertido para UTC no banco de dados (PostgreSQL exige UTC),
/// e que toda leitura do banco (UTC) seja convertida de volta para Horário de Brasília.
/// </summary>
public class BrazilianDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    private static readonly TimeZoneInfo BrazilTimeZone = GetBrazilTimeZone();

    public BrazilianDateTimeConverter() : base(
        v => ConvertToUtc(v),
        v => ConvertToBrazilianTime(v))
    {
    }

    private static TimeZoneInfo GetBrazilTimeZone()
    {
        try
        {
            // Tenta obter o fuso horário para Windows
            return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            // Tenta obter o fuso horário para Linux/Mac (IANA)
            return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        }
    }

    private static DateTime ConvertToUtc(DateTime localDateTime)
    {
        if (localDateTime.Kind == DateTimeKind.Utc)
        {
            return localDateTime;
        }

        // Se veio Unspecified ou Local, consideramos que é Horário de Brasília e convertemos para UTC
        var unspecifiedTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecifiedTime, BrazilTimeZone);
    }

    private static DateTime ConvertToBrazilianTime(DateTime utcDateTime)
    {
        // O banco retorna UTC, convertemos para Horário de Brasília
        var brazilTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, BrazilTimeZone);
        
        // Deixamos como Unspecified para que a serialização JSON no .NET não adicione offset (ex: -03:00) 
        // e o front-end apenas exiba a data exatamente como foi processada.
        return DateTime.SpecifyKind(brazilTime, DateTimeKind.Unspecified);
    }
}
