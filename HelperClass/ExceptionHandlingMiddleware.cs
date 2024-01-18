namespace BaxtureAssignAuthAPI.HelperClass
{
    // ExceptionHandlingMiddleware.cs

    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging library here)

                // Handle the exception and prepare a response
                var response = new { Message = "An error occurred.", Error = ex.Message };
                var jsonResponse = JsonConvert.SerializeObject(response);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }

}
