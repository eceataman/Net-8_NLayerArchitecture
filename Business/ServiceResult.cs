using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Services
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public List<string>? ErrorMessage { get; set; }

        //public bool IsSuccess()
        //{
        //    return ErrorMessage == null || ErrorMessage.Count == 0;
        //}
        //[JsonIgnore], JSON serileştirme (serialization) sırasında belirli bir property'nin JSON çıktısına dahil edilmemesini sağlar.örneğin issuccess false, isfail true gibi mesela product not found hatası verdi


        [JsonIgnore]
        public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0;
        [JsonIgnore]
        public bool isFail => !IsSuccess;
        [JsonIgnore]
        public HttpStatusCode Status { get; set; }
        [JsonIgnore] public string? UrlAsCreated { get; set; }
        public static ServiceResult<T> Fail(List<string>? errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T>
            {
                ErrorMessage = errorMessage,
                Status = status
            };
        }
        public static ServiceResult<T> Success(T data, HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ServiceResult<T>
            {
                Data = data,
                Status = status
            };
        }
        public static ServiceResult<T> SuccessAsCreated(T data, string urlAsCreated)
        {
            return new ServiceResult<T>
            {
                Data= data,
                Status= HttpStatusCode.Created,
                UrlAsCreated = urlAsCreated
            };
        }
        public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T>
            {
                ErrorMessage = [errorMessage],
                Status = status
            };
        }
    }

    public class ServiceResult
    {
    public List<string>? ErrorMessage { get; set; }

        //public bool IsSuccess()
        //{
        //    return ErrorMessage == null || ErrorMessage.Count == 0;
        //}
   [JsonIgnore]
    public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0;
        [JsonIgnore]
        public bool isFail => !IsSuccess;
        [JsonIgnore]
        public HttpStatusCode Status { get; set; }
    public static ServiceResult Fail(List<string> errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new ServiceResult
        {
            ErrorMessage = errorMessage,
            Status = status
        };
    }
    public static ServiceResult Success(HttpStatusCode status = HttpStatusCode.OK)
    {
        return new ServiceResult
        {
            
            Status = status
        };
    }
    public static ServiceResult Fail(string errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new ServiceResult
        {
            ErrorMessage = [errorMessage],
            Status = status
        };
    }
}
}

