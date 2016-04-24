using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Collections.Generic;
using System.IdentityModel.Tokens;

namespace SecuredBOMIApplication
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            CookieAuthenticationOptions options = new CookieAuthenticationOptions();
            options.CookieName = "martinscookie";

            app.UseCookieAuthentication(options);
            //app.UseOpenIdConnectAuthentication(BuildBioIdConnectOptions());
            app.UseOpenIdConnectAuthentication(BuildAzureOpenIdConnectOptions());
        }

        private OpenIdConnectAuthenticationOptions BuildBioIdConnectOptions()
        {
            OpenIdConnectAuthenticationOptions options = new OpenIdConnectAuthenticationOptions();
            options.ClientId = "0fbdb653-04d2-4b84-b4b4-79623bdad400";
            options.RedirectUri = "https://localhost:44300/";
            options.Authority = "https://account.bioid.com/connect";
            options.ClientSecret = "bnzkOFXjJDb6nW/8Gqn0Kbfn";
            options.PostLogoutRedirectUri = "https://localhost:44300/Home/About";

            return options;
        }

        private OpenIdConnectAuthenticationOptions BuildAzureOpenIdConnectOptions()
        {
            List<string> validTenants = new List<string>();

            //Add BOMI as a trusted Tenant
            validTenants.Add("https://sts.windows.net/65d15693-ed84-471e-88e6-b21c16a2af42/");
            //Add HSE as a trusted Tenant
            validTenants.Add("https://sts.windows.net/8573f58c-1c22-4de9-9935-dbfec34ce5f3/");

            OpenIdConnectAuthenticationOptions options = new OpenIdConnectAuthenticationOptions();
            options.ClientId = "33abe5d0-db4e-4371-9dab-d8ddfdcb5ec7";
            options.RedirectUri = "https://localhost:44300/";
            options.Authority = "https://login.microsoftonline.com/common";

            TokenValidationParameters tokenParams = new System.IdentityModel.Tokens.TokenValidationParameters();
            tokenParams.ValidIssuers = validTenants;

            options.TokenValidationParameters = tokenParams;

            return options;
        }
    }
}