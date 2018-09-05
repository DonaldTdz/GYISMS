using Microsoft.AspNetCore.Antiforgery;
using GYISMS.Controllers;

namespace GYISMS.Web.Host.Controllers
{
    public class AntiForgeryController : GYISMSControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
