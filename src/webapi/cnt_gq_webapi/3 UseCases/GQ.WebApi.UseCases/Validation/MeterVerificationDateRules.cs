namespace GQ.WebApi.UseCases.Validation;

/// <summary>Правила валидации даты поверки счётчика.</summary>
internal static class MeterVerificationDateRules
{
    internal static bool NotInFuture(DateOnly? date)
    {
        return date is null || date.Value <= DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
