using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class ExceptionEventArgs
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public ExceptionEventArgs(int code, string message = "none")
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
