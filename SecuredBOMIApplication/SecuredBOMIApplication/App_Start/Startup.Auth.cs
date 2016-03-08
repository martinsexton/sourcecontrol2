using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Collections.Generic;

namespace SecuredBOMIApplication
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            List<string> validTenants = new List<string>();

            //Add BOMI as a trusted Tenant
            validTenants.Add("https://sts.windows.net/65d15693-ed84-471e-88e6-b21c16a2af42/");
            //Add HSE as a trusted Tenant
            validTenants.Add("https://sts.windows.net/8573f58c-1c22-4de9-9935-dbfec34ce5f3/");

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = "33abe5d0-db4e-4371-9dab-d8ddfdcb5ec7",
                    //Authority = "https://login.windows.net/sextonmartingmail.onmicrosoft.com",
                    Authority = "https://login.microsoftonline.com/common",
                    PostLogoutRedirectUri = "https://localhost:44300/",
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                    {
                        //ValidateIssuer = false
                        ValidIssuers = validTenants
                    }
                }
            );
        }
    }
}