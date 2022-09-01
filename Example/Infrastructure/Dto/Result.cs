using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dto
{
    public class Result<T> : IResult<T>
    {
        public string Message { get; set; }
        public ResultStatusCode Status { get; set; }
        public T? Data { get; set; }
        public object? Error { get; set; }


        public static Result<T> Success<T>(T Data, string message)
        {
            var result = new Result<T>();
            result.Data = Data;
            result.Message = message;
            result.Status = ResultStatusCode.Success;
            return result;
        }

        public static Result<T> Failture(T Data, object error, string message)
        {
            var result = new Result<T>();
            result.Error = error;
            result.Message = message;
            result.Status = ResultStatusCode.Failture;
            result.Data = Data;
            return result;
        }
    }


}
