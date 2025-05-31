

//namespace Invx.SharedKernel.Api.Filters;
//public class ValidationFilter : ActionFilterAttribute
//{
//    public override void OnActionExecuting(ActionExecutingContext context)
//    {
//        if (!context.ModelState.IsValid)
//        {
//            var errors = context.ModelState
//                .Where(x => x.Value?.Errors.Count > 0)
//                .SelectMany(x => x.Value!.Errors)
//                .Select(x => x.ErrorMessage)
//                .ToArray();

//            var problemDetails = new ValidationProblemDetails(context.ModelState)
//            {
//                Type = "ValidationFailure",
//                Title = "Validation error",
//                Status = 400,
//                Detail = "One or more validation errors occurred",
//                Instance = context.HttpContext.Request.Path
//            };

//            context.Result = new BadRequestObjectResult(problemDetails);
//        }
//    }
//}
