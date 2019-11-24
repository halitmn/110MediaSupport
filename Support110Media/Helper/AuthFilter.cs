using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Support110Media.Helper
{
    /// <summary>
    /// Erişim için filter sınıfı. Sessionu kontrol eder.
    /// </summary>
    public class AuthFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session.GetString("admin") == null)
            {
                context.Result = new RedirectResult("/Support/SupportIndex");
            }
        }
    }


}
