using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Shared;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? Message { get; private set; }

    private Result(bool isSuccess, T? data, string? message)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
    }

    // Factory method for success
    public static Result<T> Success(T data, string? message = "Operation Successful.") => new Result<T>(true, data, message);

    // Factory method for failure
    public static Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
}
