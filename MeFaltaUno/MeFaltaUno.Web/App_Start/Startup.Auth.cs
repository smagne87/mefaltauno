using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using MeFaltaUno.Web.Models;
using System.Configuration;
using Microsoft.Owin.Security.Twitter;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using MeFaltaUno.Web.AppCode.Helpers;

namespace MeFaltaUno.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var twitterOptions = new TwitterAuthenticationOptions()
            {
                Provider = new TwitterAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:twitter:access_token", context.AccessToken, "XmlSchemaString", "Twitter"));
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:twitter:access_token_secret", context.AccessTokenSecret, "XmlSchemaString", "Twitter"));
                        return Task.FromResult(0);
                    }
                },
                ConsumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"],
                BackchannelCertificateValidator = new Microsoft.Owin.Security.CertificateSubjectKeyIdentifierValidator(new[]
                {
                    "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                    "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                    "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                    "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                    "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", // VeriSign Class 3 Primary CA - G5
                    "5168FF90AF0207753CCCD9656462A212B859723B", // DigiCert SHA2 High Assurance Server C‎A 
                    "B13EC36903F8BF4701D498261A0802EF63642BC3" // DigiCert High Assurance EV Root CA
                })
            };

            app.UseTwitterAuthentication(twitterOptions);

            var facebookOptions = new FacebookAuthenticationOptions()
            {
                Provider = new FacebookAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        // All data from facebook in this object. 
                        dynamic rawUserObjectFromFacebookAsJson = (dynamic)context.User;

                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:access_token", context.AccessToken, "XmlSchemaString", "Facebook"));
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:sex", rawUserObjectFromFacebookAsJson.gender.ToString(), "XmlSchemaString", "Facebook"));
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:first_name", rawUserObjectFromFacebookAsJson.first_name.ToString(), "XmlSchemaString", "Facebook"));
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:last_name", rawUserObjectFromFacebookAsJson.last_name.ToString(), "XmlSchemaString", "Facebook"));
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:profile_picture", string.Format("https://graph.facebook.com/{0}/picture", rawUserObjectFromFacebookAsJson.id), "XmlSchemaString", "Facebook"));
                        return Task.FromResult(0);
                    }
                },
                AppId = ConfigurationManager.AppSettings["FacebookAPPID"],
                AppSecret = ConfigurationManager.AppSettings["FacebookAPPSecret"],
                BackchannelHttpHandler = new FacebookBackChannelHandler()
            };
            facebookOptions.Scope.Add("email");

            app.UseFacebookAuthentication(facebookOptions);

            var googleOptions = new GoogleOAuth2AuthenticationOptions()
            {
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        dynamic rawUserObjectFromFacebookAsJson = (dynamic)context.User;
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:google:access_token", context.AccessToken, "XmlSchemaString", "Google"));

                        if (rawUserObjectFromFacebookAsJson.gender != null)
                            context.Identity.AddClaim(new System.Security.Claims.Claim("urn:google:sex", rawUserObjectFromFacebookAsJson.gender.ToString(), "XmlSchemaString", "Google"));

                        if (rawUserObjectFromFacebookAsJson.url != null)
                        {
                            string googleProfileUrl = rawUserObjectFromFacebookAsJson.image.url.ToString();
                            googleProfileUrl = googleProfileUrl.Substring(0, googleProfileUrl.IndexOf("?"));
                            context.Identity.AddClaim(new System.Security.Claims.Claim("urn:google:profile_picture", googleProfileUrl, "XmlSchemaString", "Google"));
                        }

                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:google:first_name", context.GivenName, "XmlSchemaString", "Google"));
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:google:last_name", context.FamilyName, "XmlSchemaString", "Google"));
                        return Task.FromResult(0);
                    }
                },
                ClientId = ConfigurationManager.AppSettings["GoogleClientID"],
                ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"],
            };

            app.UseGoogleAuthentication(googleOptions);
        }
    }
}