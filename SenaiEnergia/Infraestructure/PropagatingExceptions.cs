using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenaiEnergia.Infraestructure
{
    public class HttpException : Exception
    {
        public int StatusCode { get; set; }

        public dynamic Body { get; set; }

        public HttpException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpException(int statusCode, dynamic body)
        {
            this.StatusCode = statusCode;
            this.Body = body;
        }


    }

    public class NotFoundException : HttpException
    {
        public NotFoundException() : base(404)
        {
        }
        public NotFoundException(string message) : base(404, message)
        {

        }

    }
}
