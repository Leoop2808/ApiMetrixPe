using ApiMetrixPe.App_Start;
using MetrixPeDataAccess.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiMetrixPe.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            /*decateca_db_transacEntities ctxBD = new decateca_db_transacEntities();

            string clientId = string.Empty;
            string clientSecret = string.Empty;
            
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {

                //new LoggerRepository().registroLogError(null, null, "invalid_client", "Client credentials could not be retrieved through the Authorization header.");

                context.SetError("invalid_client", "Client credentials could not be retrieved through the Authorization header.");
                return Task.FromResult<object>(null);
            }*/

            //Check the existence of by calling the ValidateClient method
            /*Aplicativo client = ctxBD.Aplicativo.FirstOrDefault(user =>
                                    user.clientname == clientId &&
                                    user.clientsecret == clientSecret);


            if (client == null)
            {
                //new LoggerRepository().registroLogError(null, null, "invalid_client", "Client credentials are invalid.");

                // Client could not be validated.
                context.SetError("invalide_client", "Client credentials are invalid.");
                return Task.FromResult<object>(null);
            }
            else
            {
                if (client.Activo == null || client.Activo == false)
                {
                    new LoggerRepository().registroLogError(null, null, "invalid_client", "Client is inactive.");

                    context.SetError("invalid_client", "Client is inactive.");
                    return Task.FromResult<object>(null);
                }
                // Client has been verified.
                context.OwinContext.Set<Aplicativo>("ta:client", client);

                context.OwinContext.Set<string>("ta:clientAllowedOrigin", client.allowedorigin);
                context.OwinContext.Set<string>("ta:clientRefreshTokenLifeTime", client.refreshtokenlifetime.ToString());
                context.Validated();
                return Task.FromResult<object>(null);
            }*/
            context.Validated();
            return Task.FromResult<object>(null);
        }



        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //var clienteAplicativo = context.OwinContext.Get<Aplicativo>("ta:client");
            try
            {
                MetrixPeBDEntities ctxBD = new MetrixPeBDEntities();


                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();


                usuario user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    //new LoggerRepository().RegistroLogLogin(context.UserName, clienteAplicativo.clientid, "invalid_grant -- The user name or password is incorrect.", null, null);

                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                else
                {
                    if (user.activo == false || user.activo == null)
                    {
                        //new LoggerRepository().RegistroLogLogin(context.UserName, clienteAplicativo.clientid, "invalid_user -- The user is inactive.", null, null);

                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }
                    if (user.eliminado == true)
                    {
                        //new LoggerRepository().RegistroLogLogin(context.UserName, clienteAplicativo.clientid, "invalid_user -- The user was deleted.", null, null);

                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }
                }

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);



                oAuthIdentity.AddClaim(new Claim("user_id", user.id_usuario.ToString()));
                oAuthIdentity.AddClaim(new Claim("user_code", user.cod_usuario.ToString()));
                //oAuthIdentity.AddClaim(new Claim("clienteid_aplicativo", clienteAplicativo.clientid.ToString()));

                /*Rol rol = ctxBD.Rol.Where(x => x.idRol == user.IdRol).FirstOrDefault();

                if (rol.activo == null || rol.activo == false)
                {
                    new LoggerRepository().RegistroLogLogin(context.UserName, clienteAplicativo.clientid, "invalid_user_rol -- User has no an active role", null, null);
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                if (rol.eliminado == true)
                {
                    new LoggerRepository().RegistroLogLogin(context.UserName, clienteAplicativo.clientid, "invalid_user_rol -- User has a deleted role.", null, null);
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, rol.codRol));*/


                ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);
                AuthenticationProperties properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "Username", user.username
                    }
                });
                //var options = new OAuthAuthorizationServerOptions();

                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);


                //var tokenContext = new AuthenticationTokenCreateContext(context.OwinContext, options.AccessTokenFormat, ticket);
                /*var tokenContext = new AuthenticationTokenCreateContext(context.OwinContext, context.Options.AccessTokenFormat, ticket);
                await context.Options.AccessTokenProvider.CreateAsync(tokenContext);*/
                //log login
                //new LoggerRepository().RegistroLogLogin(context.UserName, clienteAplicativo.clientid, "Inicio sesion exitoso.", null, null);

            }
            catch (Exception)
            {
                //motivo error, error interno al validar el inicio de la sesion
                //new LoggerRepository().RegistroLogLogin(context.UserName, clienteAplicativo.clientid, "error_server -- " + ex.Message, ex.StackTrace, null);

                //log login
                context.SetError("error_server", "Error server");
                return;
            }

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(usuario userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "username", userName.UserName.ToString() },
            };
            return new AuthenticationProperties(data);
        }
    }
}