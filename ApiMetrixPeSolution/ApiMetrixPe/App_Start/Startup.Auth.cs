using ApiMetrixPe.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(ApiMetrixPe.App_Start.Startup))]
namespace ApiMetrixPe.App_Start
{
    public class Startup
    {


        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                //The Path For generating the Toekn
                TokenEndpointPath = new PathString("/Token"),
                //Setting the Token Expired Time (24 HOURS)
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(24),
                //MyAuthorizationServerProvider class will validate the user credentials
                Provider = new ApplicationOAuthProvider(),

                AccessTokenFormat = new TicketDataFormat(app.CreateDataProtector(
                            typeof(OAuthAuthorizationServerMiddleware).Namespace,
                            "Access_Token", "v1")),
                RefreshTokenFormat = new TicketDataFormat(app.CreateDataProtector(
                            typeof(OAuthAuthorizationServerMiddleware).Namespace,
                            "Refresh_Token", "v1")),
                AccessTokenProvider = new AuthenticationTokenProvider(),
                RefreshTokenProvider = new AuthenticationTokenProvider(),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
            };

            //app.UseOAuthAuthorizationServer(options);
            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseOAuthBearerTokens(OAuthOptions);


            OAuthBearerOptions = new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = OAuthOptions.AccessTokenFormat,
                AccessTokenProvider = OAuthOptions.AccessTokenProvider,
                AuthenticationMode = OAuthOptions.AuthenticationMode,
                AuthenticationType = OAuthOptions.AuthenticationType,
                Description = OAuthOptions.Description,
                Provider = new CustomBearerAuthenticationProvider()
            };

            OAuthBearerAuthenticationExtensions.UseOAuthBearerAuthentication(app, OAuthBearerOptions);

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }

        public class CustomBearerAuthenticationProvider : OAuthBearerAuthenticationProvider
        {
            public override Task ValidateIdentity(OAuthValidateIdentityContext context)
            {
                var claims = context.Ticket.Identity.Claims;
                if (context.Ticket.Properties.ExpiresUtc.Value > DateTime.UtcNow)
                {
                    if (claims.Count() == 0 || claims.Any(claim => claim.Issuer != "Facebook" && claim.Issuer != "Google" && claim.Issuer != "LOCAL_AUTHORITY"))
                        context.Rejected();
                }
                return Task.FromResult<object>(null);
            }
        }
    }
}