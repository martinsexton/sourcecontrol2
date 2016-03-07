using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace SecuredBOMIApplication
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = "33abe5d0-db4e-4371-9dab-d8ddfdcb5ec7",
                    Authority = "https://login.windows.net/sextonmartingmail.onmicrosoft.com",
                    PostLogoutRedirectUri = "https://localhost:44300/"
                }
            );
        }
    }
}