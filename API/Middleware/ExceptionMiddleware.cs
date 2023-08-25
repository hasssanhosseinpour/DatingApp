using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        //Because it is a middleware we use something called RequestDelegate.
        // we inject ILogger to hadle the exceptions.
        //env will allow us to see our application is running in Development mode or Production mode.
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,
             IHostEnvironment env )
        {
            _env = env;
            _logger = logger;
            _next = next;
            
        }
        //This method has to be called with this name,InvokeAsync,
        //because it is a framework and we are telling framework that this is middleware.
        //The HttpContext context gives us access to the http request that currently been passed through middleware
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // No logic needed here. logics are in catch section.
                await _next(context);
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode,ex.Message,"Internal Server Error");

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);

            }
        }
    }
}