using Microsoft.AspNetCore.Mvc;
using NutrionE.Models;

namespace NutrionE.Util
{
    public static class OperationResultExtensions
    {
        public static ActionResult ToActionResult(this OperationResult operationResult)
        {
            switch (operationResult.Code)
            {
                case OperationResult.ResultCode.INVALID_DATA:
                    return new BadRequestResult();

                case OperationResult.ResultCode.INVALID_STATE:
                    return new ConflictResult();

                case OperationResult.ResultCode.NOT_FOUND:
                    return new NotFoundResult();

                case OperationResult.ResultCode.SUCCESS:
                    return new NoContentResult();

                case OperationResult.ResultCode.UNAUTHORIZED:
                    return new UnauthorizedResult();
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        public static ActionResult ToActionResult<T>(this OperationResult<T> operationResult)
        {
            switch (operationResult.Code)
            {
                case OperationResult.ResultCode.INVALID_DATA:
                    return new BadRequestObjectResult(operationResult.Result);

                case OperationResult.ResultCode.INVALID_STATE:
                    return new ConflictObjectResult(operationResult.Result);

                case OperationResult.ResultCode.NOT_FOUND:
                    return new NotFoundObjectResult(operationResult.Result);

                case OperationResult.ResultCode.SUCCESS:
                    return new OkObjectResult(operationResult.Result);

                case OperationResult.ResultCode.UNAUTHORIZED:
                    return new UnauthorizedResult();
            }
            var internalError = new ObjectResult(operationResult.Result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return internalError;
        }
    }
}