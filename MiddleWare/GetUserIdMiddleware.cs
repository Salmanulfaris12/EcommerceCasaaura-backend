using System.Security.Claims;

namespace CasaAura.MiddleWare
{
    public class GetUserIdMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<GetUserIdMiddleware> _logger;

        public GetUserIdMiddleware(RequestDelegate next ,ILogger<GetUserIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if(context.User.Identity?.IsAuthenticated==true)
            {
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

                if(idClaim != null)
                {
                    context.Items["UserId"]=idClaim.Value;
                }
                else
                {
                    _logger.LogWarning("'NameIdentifier' not found in JWT Token");
                }
            }
            await _next(context);
        }
        
    }
}
