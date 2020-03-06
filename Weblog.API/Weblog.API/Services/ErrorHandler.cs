using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Weblog.API.Models;

namespace Weblog.API.Services
{
    public class ErrorHandler
    {
        internal static UnprocessableEntityObjectResult UnprocessableEntity(
                                            ModelStateDictionary modelState,
                                            HttpContext httpContext)
        {
            var problemDetails = new ValidationProblemDetails(modelState)
            {
                Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
                Title = "One or more model validation errors occurred.",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = "See the errors property for details.",
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            return new UnprocessableEntityObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        }
    }
}
