using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Poseidon.AuthorizationManager;

namespace AnagraficaAPI.Middleware
{
    public class AuthorizationManagerActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IAuthorizationManager authorizationManager = context.HttpContext.RequestServices.GetService<IAuthorizationManager>();
            if (authorizationManager != null)
                await authorizationManager.LoadUserProfile();
            await next();
        }
    }
}
