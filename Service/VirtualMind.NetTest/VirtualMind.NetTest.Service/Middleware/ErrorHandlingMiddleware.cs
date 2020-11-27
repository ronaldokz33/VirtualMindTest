using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;


namespace VirtualMind.NetTest.Service.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var status = HttpStatusCode.InternalServerError;
            string errorCode = String.Concat(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            if (ex is UnauthorizedAccessException)
            {
                status = HttpStatusCode.Unauthorized;
            }
            else
            {
                using (StreamWriter sw = new StreamWriter("errors.txt", true))
                {
                    sw.WriteLine("--------------------------");
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                    sw.WriteLine($"errorCode: {errorCode}");
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace);
                }
            }

            context.Response.StatusCode = (int)status;

            if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Credentials"))
                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            if (context.Response.ContentType == null)
                context.Response.ContentType = "application/json";

            var result = JsonConvert.SerializeObject(new { status = context.Response.StatusCode, code = errorCode, message = ex.Message });
            return context.Response.WriteAsync(result);
        }
    }
}
