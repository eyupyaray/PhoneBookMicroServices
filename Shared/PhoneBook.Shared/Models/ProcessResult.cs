using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Shared.Models
{
    public class ProcessResult<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public List<string> Messages { get; set; }

        public static ProcessResult<T> Success (T data, int statusCode)
        {
            return new ProcessResult<T> { Data = data, StatusCode = statusCode, IsSuccess = true };
        }

        public static ProcessResult<T> Success( int statusCode)
        {
            return new ProcessResult<T> { Data = default(T), StatusCode = statusCode, IsSuccess = true };
        }

        public static ProcessResult<T> Error(List<string> messages, int statusCode)
        {
            return new ProcessResult<T> { Messages = messages, StatusCode = statusCode, IsSuccess = false };
        }

        public static ProcessResult<T> Error(string message, int statusCode)
        {
            return new ProcessResult<T> { Messages = new List<string>() { message }, StatusCode = statusCode, IsSuccess = false };
        }
    }
}
