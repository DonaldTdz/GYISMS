using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYISMS.Web.Host.Startup
{
    public class Jump404Middleware
    {
        private readonly RequestDelegate next;

        public Jump404Middleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await next.Invoke(context);

            var response = context.Response;

            //如果是404就跳转到主页
            if (response.StatusCode == 404)
            {
                response.Redirect("/");
            }
        }
    }

    public static class Jump404MiddlewareExtension
    {
        public static void UseJump404(this IApplicationBuilder app)
        {
            app.UseMiddleware<Jump404Middleware>();
        }
    }
}
