namespace NutrionE.Models
{
    public class OperationResult
    {
        public enum ResultCode
        { SUCCESS, GENERIC_ERROR, NOT_FOUND, INVALID_DATA, INVALID_STATE, UNAUTHORIZED }

        public ResultCode Code { get; set; }

        public static OperationResult Success()
        {
            return CodeResult(ResultCode.SUCCESS);
        }

        public static OperationResult GenericError()
        {
            return CodeResult(ResultCode.GENERIC_ERROR);
        }

        public static OperationResult NotFound()
        {
            return CodeResult(ResultCode.NOT_FOUND);
        }

        public static OperationResult InvalidData()
        {
            return CodeResult(ResultCode.INVALID_DATA);
        }

        public static OperationResult InvalidState()
        {
            return CodeResult(ResultCode.INVALID_STATE);
        }

        public static OperationResult Unauthorized()
        {
            return CodeResult(ResultCode.UNAUTHORIZED);
        }

        public static OperationResult CodeResult(ResultCode resultCode)
        {
            return new OperationResult { Code = resultCode };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Result { get; set; }

        public static OperationResult<T> Success(T result)
        {
            return CodeResult(ResultCode.SUCCESS, result);
        }

        public static OperationResult<T> NotFound(T result)
        {
            return CodeResult(ResultCode.NOT_FOUND, result);
        }

        public static OperationResult<T> GenericError(T result)
        {
            return CodeResult(ResultCode.GENERIC_ERROR, result);
        }

        public static OperationResult<T> InvalidData(T result)
        {
            return CodeResult(ResultCode.INVALID_DATA, result);
        }

        public static OperationResult<T> InvalidState(T result)
        {
            return CodeResult(ResultCode.INVALID_STATE, result);
        }

        public static OperationResult<T> Unauthorized(T result)
        {
            return CodeResult(ResultCode.UNAUTHORIZED, result);
        }

        public static OperationResult<T> CodeResult(ResultCode resultCode, T result)
        {
            return new OperationResult<T> { Code = resultCode, Result = result };
        }
    }
}