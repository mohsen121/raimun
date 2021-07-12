using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raimun.Core.Common.Models
{
    public class ApiResponse<T> where T : class
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResponse<T> GetSuccessResponse(T data)
        {
            return new ApiResponse<T>
            {
                StatusCode = 200,
                Data = data
            };
        }
    }
}
