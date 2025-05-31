//using Microsoft.AspNetCore.Mvc;

//namespace Invx.SharedKernel.Api.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public abstract class BaseApiController : ControllerBase
//{
//    protected IActionResult HandleResult<T>(Result<T> result)
//    {
//        return result.IsSuccess switch
//        {
//            true when result.Value is not null => Ok(result.Value),
//            true when result.Value is null => NotFound(),
//            false when result.Error?.Type == ErrorType.Validation => BadRequest(CreateProblemDetails(result.Error)),
//            false when result.Error?.Type == ErrorType.NotFound => NotFound(CreateProblemDetails(result.Error)),
//            false when result.Error?.Type == ErrorType.Unauthorized => Unauthorized(CreateProblemDetails(result.Error)),
//            false when result.Error?.Type == ErrorType.Forbidden => Forbid(),
//            _ => BadRequest(CreateProblemDetails(result.Error))
//        };
//    }

//    protected IActionResult HandleResult(Result result)
//    {
//        return result.IsSuccess switch
//        {
//            true => Ok(),
//            false when result.Error?.Type == ErrorType.Validation => BadRequest(CreateProblemDetails(result.Error)),
//            false when result.Error?.Type == ErrorType.NotFound => NotFound(CreateProblemDetails(result.Error)),
//            false when result.Error?.Type == ErrorType.Unauthorized => Unauthorized(CreateProblemDetails(result.Error)),
//            false when result.Error?.Type == ErrorType.Forbidden => Forbid(),
//            _ => BadRequest(CreateProblemDetails(result.Error))
//        };
//    }

//    private static ProblemDetails CreateProblemDetails(Error? error)
//    {
//        return new ProblemDetails
//        {
//            Title = error?.Code ?? "Error",
//            Detail = error?.Message ?? "An error occurred",
//            Type = error?.Type.ToString()
//        };
//    }
//}