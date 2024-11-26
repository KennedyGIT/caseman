using core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace caseman.user.api.Middleware
{
    public class FirstTimeLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public FirstTimeLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    var user = await userManager.GetUserAsync(context.User);
                    if (user != null && user.FirstTimeLogin)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("First-time login detected. Please reset your password.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
