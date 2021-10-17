using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CheckOutTest.Web.Configuration.Middleware
{
    public class ApiKeyAuth : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public ApiKeyAuth(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                          ILoggerFactory logger,
                                          UrlEncoder encoder,
                                          ISystemClock clock) : base(options, logger, encoder, clock) { }

        /// <summary>
        /// Handles the authentication, asynchronously.
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.TryGetValue("ApiKey", out var value))
            {
                if (value.Count > 0)
                {
                    var apiKey = value[0];

                    //This can be grabbed from the secret store/ID server etc
                    if (apiKey == "1f964f4c-ee29-4c75-bf05-56f750c6fa95")
                    {

                        ClaimsIdentity identity = new ClaimsIdentity(Scheme.Name);

                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return AuthenticateResult.Success(ticket);
                    }
                }
            }

            return AuthenticateResult.Fail("Invalid");
        }
    }
}
