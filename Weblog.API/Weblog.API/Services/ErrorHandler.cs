using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Weblog.API.Models;

namespace Weblog.API.Services
{
    public class ErrorHandler
    {
        internal static UnprocessableEntityObjectResult UnprocessableEntity(
            ControllerBase controller, Exception exception)
        {
            controller.ModelState.AddModelError(exception?.InnerException.Message,
                                        exception,
                                        controller.MetadataProvider.GetMetadataForType(
                                            typeof(UserForCreationDto)));

            var problemDetails = new ValidationProblemDetails(controller.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
                Title = "One or more model validation errors occurred.",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = "See the errors property for details.",
                Instance = controller.HttpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", controller.HttpContext.TraceIdentifier);

            return new UnprocessableEntityObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        }
    }
}
