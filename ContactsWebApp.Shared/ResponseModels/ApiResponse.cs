using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsWebApp.Shared.ResponseModels
{
    public class ApiResponse<T>
    {
        public bool IsSucces { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }

        public ApiResponse()
        {
            
        }

        public ApiResponse(bool isSucces, string message, T data)
        {
            IsSucces = isSucces;
            Message = message;
            Data = data;
        }
    }
    public class ApiResponse
    {
        public bool IsSucces { get; set; }
        public string Message { get; set; } = null!;

        public ApiResponse()
        {

        }

        public ApiResponse(bool isSucces, string message)
        {
            IsSucces = isSucces;
            Message = message;
        }
    }
}
