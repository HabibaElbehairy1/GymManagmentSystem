using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Common
{
    public record Result(bool Success, string? Message = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result Ok() => new(true);
        public static Result Fail(string message , ResultKind kind) => new(false, message, kind);
        public static Result Validation(string message) => new(false, message, ResultKind.ValidationFailed);
        public static Result NotFound(string message = "Not Found") => new(false , message , ResultKind.NotFound);


    }
    public record Result<T>(bool success,T? value, string? message = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new(true ,value);
        public static Result<T> Fail(string message, ResultKind kind) => new(  false,default , message, kind);
        public static Result<T> Validation(string message) => new( false,default , message, ResultKind.ValidationFailed);
        public static Result<T> NotFound(string message = "Not Found" ) => new(  false,default , message, ResultKind.NotFound);


    }
}
