using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();

        // Log the request header
        Log.Error($"Http Request Information:{Environment.NewLine}" +
                               $"Schema:{context.Request.Scheme} " +
                               $"Host: {context.Request.Host} " +
                               $"Path: {context.Request.Path} " +
                               $"QueryString: {context.Request.QueryString} " +
                               $"Request Body: {await GetRequestBody(context.Request)}");

        await _next(context);
    }

    private async Task<string> GetRequestBody(HttpRequest request)
    {
        request.Body.Seek(0, SeekOrigin.Begin);
        string body = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }
}
