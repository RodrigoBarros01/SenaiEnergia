using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenaiEnergia.Infraestructure
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (HttpException e)
            {
                context.Response.StatusCode = e.StatusCode;
                var b = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new {
                    Erro = e.Body.ToString()
                }));
                context.Response.Body.Write(b, 0, b.Length);

            }
        }
    }
}
