using Luck.Framework.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Luck.AspNetCore.ApiResults
{
    public interface IApiResultWrapAttribute : IActionFilter, IFilterMetadata
    {
    }
    public class ApiResultWrapAttribute : ActionFilterAttribute, IApiResultWrapAttribute
    {
        public const string InternalServerError = "Internal Server Error";
        public const string InvalidParameter = "Invalid Parameter";

        public virtual Exception OnException(Exception ex)
        {
            return ex;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!context.ModelState.IsValid)
            {
                var error = string.Join(", ", context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()
                                  ?.CreateLogger(context.Controller.GetType());
                logger?.LogDebug("InvalidParameter. {error}", error);

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(new ApiResult(InvalidParameter, error));
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (context.Exception != null)
            {
                var ex = OnException(context.Exception);
                if (ex is NotFoundException)
                {
                    context.Exception = null;
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                else
                {
                    var hostEnvironment = context.HttpContext.RequestServices.GetService<IHostEnvironment>();
                    var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
                                        .CreateLogger(context.Controller.GetType());

                    string errorCode;
                    if (ex is BusinessException businessException)
                    {
                        errorCode = businessException.ErrorCode ?? string.Empty;

                        logger?.LogDebug(businessException, $"action failed due to business exception");
                    }
                    else
                    {
                        errorCode = InternalServerError;

                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        logger?.LogError(ex, $"action failed due to exception");
                    }

                    context.Exception = null;
                    //if production do not response stacktrace.
                    if (hostEnvironment is not null && hostEnvironment.IsProduction())
                    {
                        context.Result = new JsonResult(new ApiResult(errorCode, "服务器内部错误"));
                    }
                    else
                    {
                        var exception = ex.GetBaseException() ?? ex;
                        context.Result = new JsonResult(new ApiResultWithStackTrace(errorCode, exception.Message, exception.StackTrace));
                    }
                }
            }
            else
            {
                var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
                var attributes = descriptor.MethodInfo.CustomAttributes;

                if (attributes.Any(a => a.AttributeType == typeof(DisableApiResultWrapAttribute)))
                {
                    return;
                }
                else
                {
                    var actionResult = GetValue(context.Result);
                    context.Result = new JsonResult(new ApiResult(actionResult));
                }
            }
        }

        private static object? GetValue(IActionResult? actionResult)
        {
            return (actionResult as JsonResult)?.Value ?? (actionResult as ObjectResult)?.Value;
        }
    }
}