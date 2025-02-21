using App.Services.ExceptionHandlers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

public class CriticalExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is CriticalException)
        {
            Console.WriteLine("Hata ile ilgili SMS gönderildi.");
            return ValueTask.FromResult(true); // İşlendiğini belirtmek için true dönmelisiniz.
        }

        return ValueTask.FromResult(false);
    }
}
