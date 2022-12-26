using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace EPGP.API.Filters
{
    public class NotFoundExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                // Customize this object to fit your needs
                var result = new ObjectResult(new
                {
                    context.Exception.Message, // Or a different generic message
                    context.Exception.Source,
                    ExceptionType = context.Exception.GetType().FullName,
                })
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };

                // Set the result
                context.Result = result;
            }
        }
    }
}
