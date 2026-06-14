using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UnitConverterAPI.Models.Common;

namespace UnitConverterAPI.Filters;

public class ApiResponseWrapperFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if(context.Result is ObjectResult objectResult && objectResult.Value is not null)
        {
            var valueType = objectResult.Value.GetType();

            bool isAlreadyWrapped = valueType.IsGenericType &&
                                    valueType.GetGenericTypeDefinition() == typeof(ApiResponse<>);
            bool isProblemDetails = objectResult.Value is ProblemDetails;

            if(!isAlreadyWrapped && !isProblemDetails)
            {
                var wrappedResponse = ApiResponse<object>.CreateSuccess(objectResult.Value);
                objectResult.Value = wrappedResponse;
            }
        }

        await next();
    }
}