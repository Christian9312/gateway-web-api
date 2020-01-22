using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.Domain.Communication
{
    public class Response
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; }

        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
