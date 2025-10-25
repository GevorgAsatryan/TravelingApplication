using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace TravelingApplication
{
    [Authorize]
    public class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {

            if (authorizeResult.Forbidden
                && authorizeResult.AuthorizationFailure!.FailedRequirements
                    .OfType<Show404Requirement>().Any())
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }

        public class Show404Requirement : IAuthorizationRequirement { }
}
