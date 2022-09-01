using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dto
{
    public interface IResult
    {
        public string Message { get; set; }
        public ResultStatusCode Status { get; set; }

    }

    public interface IResult<T>: IResult
    {
        public  T Data { get; set; }
        public  object Error { get; set; }

    }
}
