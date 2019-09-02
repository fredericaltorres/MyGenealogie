using Microsoft.AspNetCore.Http;
using MyGenealogie.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGenealogie.Web.Middleware
{
    /// <summary>
    /// https://www.tpeczek.com/2019/01/aspnet-core-middleware-and-authorization.html
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly PersonDB personDB;
        public AuthenticationMiddleware(RequestDelegate next, IPersonDB personDB)
        {
            this._next = next;
            this.personDB = personDB as PersonDB;
        }
        public async Task Invoke(HttpContext context)
        {
            System.Console.WriteLine($"{context.Request.Path}");

            if (ShouldApplyAuthentication(context))
            {
                var userName = context.Request.Headers["Username"];
                var password = context.Request.Headers["Password"];

                if (string.IsNullOrEmpty(userName))
                {
                    var m = $"Authentication failed username cannot be null or empty";
                    await SetError(context, m);
                    return;
                }
                if (string.IsNullOrEmpty(password))
                {
                    var m = $"Authentication failed password cannot be null or empty";
                    await SetError(context, m);
                    return;
                }
                var authenticatedPerson = this.personDB.GetPersonByUsername(userName);
                if (!authenticatedPerson.VerifyPassword(password))
                {
                    var m = $"Authentication failed for username:{userName}";
                    await SetError(context, m);
                    return;
                }
            }
            await _next(context); // Call the next delegate/middleware in the pipeline
        }

        private static async Task SetError(HttpContext context, string m, int statusCode = 401 /* Unauthorized */)
        {
            System.Console.WriteLine(m);
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync($@"{{ ""message"":""{m}"" }}");
        }

        private static bool ShouldApplyAuthentication(HttpContext context)
        {
            if(context.Request.Method != "GET") {
                return context.Request.Path.StartsWithSegments(new PathString("/api/MyGenealogie"));
            }
            return false;            
        }
    }
}
