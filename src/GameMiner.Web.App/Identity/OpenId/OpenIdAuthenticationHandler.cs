/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GameMiner.Web.App.Identity.OpenId
{
    public class OpenIdAuthenticationHandler<TOptions> : RemoteAuthenticationHandler<TOptions>
        where TOptions : OpenIdAuthenticationOptions, new()
    {
        public OpenIdAuthenticationHandler(
            IOptionsMonitor<TOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            // Always extract the "state" parameter from the query string.
            var state = Request.Query[OpenIdAuthenticationConstants.Parameters.State];
            if (string.IsNullOrEmpty(state))
            {
                return HandleRequestResult.Fail("The authentication response was rejected " +
                                                "because the state parameter was missing.");
            }

            var properties = Options.StateDataFormat.Unprotect(state);
            if (properties == null)
            {
                return HandleRequestResult.Fail("The authentication response was rejected " +
                                                "because the state parameter was invalid.");
            }

            // Validate the anti-forgery token.
            if (!ValidateCorrelationId(properties))
            {
                return HandleRequestResult.Fail("The authentication response was rejected " +
                                                "because the anti-forgery token was invalid.");
            }

            OpenIdAuthenticationMessage message;

            // OpenID 2.0 responses MUST necessarily be made using either GET or POST.
            // See http://openid.net/specs/openid-authentication-2_0.html#anchor4
            if (!string.Equals(Request.Method, "GET", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                return HandleRequestResult.Fail("The authentication response was rejected because it was made " +
                                                "using an invalid method: make sure to use either GET or POST.");
            }

            if (string.Equals(Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
            {
                message = new OpenIdAuthenticationMessage(Request.Query);
            }

            else
            {
                // OpenID 2.0 responses MUST include a Content-Type header when using POST.
                // See http://openid.net/specs/openid-authentication-2_0.html#anchor4
                if (string.IsNullOrEmpty(Request.ContentType))
                {
                    return HandleRequestResult.Fail("The authentication response was rejected because " +
                                                    "it was missing the mandatory 'Content-Type' header.");
                }

                // May have media/type; charset=utf-8, allow partial match.
                if (!Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
                {
                    return HandleRequestResult.Fail("The authentication response was rejected because an invalid Content-Type header " +
                                                    "was received: make sure to use 'application/x-www-form-urlencoded'.");
                }

                message = new OpenIdAuthenticationMessage(await Request.ReadFormAsync(Context.RequestAborted));
            }

            // Ensure that the current request corresponds to an OpenID 2.0 assertion.
            if (!string.Equals(message.Namespace, OpenIdAuthenticationConstants.Namespaces.OpenId, StringComparison.Ordinal))
            {
                return HandleRequestResult.Fail("The authentication response was rejected because it was missing the mandatory " +
                                                "'openid.ns' parameter or because an unsupported version of OpenID was used.");
            }

            // Stop processing the message if the authentication process was cancelled by the user.
            if (string.Equals(message.Mode, OpenIdAuthenticationConstants.Modes.Cancel, StringComparison.Ordinal))
            {
                return HandleRequestResult.Fail("The authentication response was rejected because " +
                                                "the operation was cancelled by the user.");
            }

            // Stop processing the message if an error was returned by the provider.
            else if (string.Equals(message.Mode, OpenIdAuthenticationConstants.Modes.Error, StringComparison.Ordinal))
            {
                if (string.IsNullOrEmpty(message.Error))
                {
                    return HandleRequestResult.Fail("The authentication response was rejected because an " +
                                                    "unspecified error was returned by the identity provider.");
                }

                return HandleRequestResult.Fail("The authentication response was rejected because " +
                                               $"an error was returned by the identity provider: {message.Error}.");
            }

            // At this point, stop processing the message if the assertion was not positive.
            else if (!string.Equals(message.Mode, OpenIdAuthenticationConstants.Modes.IdRes, StringComparison.Ordinal))
            {
                return HandleRequestResult.Fail("The authentication response was rejected because " +
                                                "the identity provider declared it as invalid.");
            }

            var address = QueryHelpers.AddQueryString(uri: properties.Items[OpenIdAuthenticationConstants.Properties.ReturnTo],
                                                      name: OpenIdAuthenticationConstants.Parameters.State, value: state);

            // Validate the return_to parameter by comparing it to the address stored in the properties.
            // See http://openid.net/specs/openid-authentication-2_0.html#verify_return_to
            if (!string.Equals(message.ReturnTo, address, StringComparison.Ordinal))
            {
                return HandleRequestResult.Fail("The authentication response was rejected because the return_to parameter was invalid.");
            }

            // Make sure the OpenID 2.0 assertion contains an identifier.
            if (string.IsNullOrEmpty(message.ClaimedIdentifier))
            {
                return HandleRequestResult.Fail("The authentication response was rejected because it " +
                                                "was missing the mandatory 'claimed_id' parameter.");
            }

            var identity = new ClaimsIdentity(Scheme.Name);

            // Add the claimed identifier to the identity.
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, message.ClaimedIdentifier, ClaimValueTypes.String, Options.ClaimsIssuer));
            identity.AddClaim(new Claim(ClaimTypes.Name, message.ClaimedIdentifier, ClaimValueTypes.String, Options.ClaimsIssuer));

            // Add the most common attributes to the identity.
            var attributes = message.GetAttributes();
            foreach (var attribute in attributes)
            {
                // http://axschema.org/contact/email
                if (string.Equals(attribute.Key, OpenIdAuthenticationConstants.Attributes.Email, StringComparison.Ordinal))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Email, attribute.Value, ClaimValueTypes.Email, Options.ClaimsIssuer));
                }

                // http://axschema.org/namePerson
                else if (string.Equals(attribute.Key, OpenIdAuthenticationConstants.Attributes.Name, StringComparison.Ordinal))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Name, attribute.Value, ClaimValueTypes.String, Options.ClaimsIssuer));
                }

                // http://axschema.org/namePerson/first
                else if (string.Equals(attribute.Key, OpenIdAuthenticationConstants.Attributes.Firstname, StringComparison.Ordinal))
                {
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, attribute.Value, ClaimValueTypes.String, Options.ClaimsIssuer));
                }

                // http://axschema.org/namePerson/last
                else if (string.Equals(attribute.Key, OpenIdAuthenticationConstants.Attributes.Lastname, StringComparison.Ordinal))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Surname, attribute.Value, ClaimValueTypes.String, Options.ClaimsIssuer));
                }
            }

            // Create a ClaimTypes.Name claim using ClaimTypes.GivenName and ClaimTypes.Surname
            // if the http://axschema.org/namePerson attribute cannot be found in the assertion.
            if (!identity.HasClaim(claim => string.Equals(claim.Type, ClaimTypes.Name, StringComparison.OrdinalIgnoreCase)) &&
                 identity.HasClaim(claim => string.Equals(claim.Type, ClaimTypes.GivenName, StringComparison.OrdinalIgnoreCase)) &&
                 identity.HasClaim(claim => string.Equals(claim.Type, ClaimTypes.Surname, StringComparison.OrdinalIgnoreCase)))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, $"{identity.FindFirst(ClaimTypes.GivenName).Value} " +
                                                             $"{identity.FindFirst(ClaimTypes.Surname).Value}",
                                            ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var ticket = await CreateTicketAsync(identity, properties, message.ClaimedIdentifier, attributes);
            if (ticket == null)
            {
                Logger.LogInformation("The authentication process was skipped because returned a null ticket was returned.");

                return HandleRequestResult.SkipHandler();
            }

            return HandleRequestResult.Success(ticket);
        }

        protected virtual async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity, AuthenticationProperties properties,
            string identifier, IDictionary<string, string> attributes)
        {
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Scheme.Name);

            var context = new OpenIdAuthenticatedContext(Context, Scheme, Options, ticket);

            // Copy the attributes to the context object.
            foreach (var attribute in attributes)
            {
                context.Attributes.Add(attribute);
            }

            await Options.Events.Authenticated(context);

            // Note: return the authentication ticket associated
            // with the notification to allow replacing the ticket.
            return context.Ticket;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await Task.FromResult(true);

            if (string.IsNullOrEmpty(Options.AuthenticationEndpoint))
            {
                throw new InvalidOperationException("The OpenID 2.0 authentication middleware was unable to retrieve " +
                                                    "the authentication endpoint address from the discovery document.");
            }

            // Determine the realm using the current address
            // if one has not been explicitly provided;
            var realm = Options.Realm;
            if (string.IsNullOrEmpty(realm))
            {
                realm = Request.Scheme + "://" + Request.Host + OriginalPathBase;
            }

            // Use the current address as the final location where the user agent
            // will be redirected to if one has not been explicitly provided.
            if (string.IsNullOrEmpty(properties.RedirectUri))
            {
                properties.RedirectUri = Request.Scheme + "://" + Request.Host +
                                         OriginalPathBase + Request.Path + Request.QueryString;
            }

            // Store the return_to parameter for later comparison.
            properties.Items[OpenIdAuthenticationConstants.Properties.ReturnTo] =
                Request.Scheme + "://" + Request.Host +
                OriginalPathBase + Options.CallbackPath;

            // Generate a new anti-forgery token.
            GenerateCorrelationId(properties);

            // Create a new message containing the OpenID 2.0 request parameters.
            // See http://openid.net/specs/openid-authentication-2_0.html#requesting_authentication
            var message = new OpenIdAuthenticationMessage
            {
                ClaimedIdentifier = "http://specs.openid.net/auth/2.0/identifier_select",
                Identity = "http://specs.openid.net/auth/2.0/identifier_select",
                Mode = OpenIdAuthenticationConstants.Modes.CheckIdSetup,
                Namespace = OpenIdAuthenticationConstants.Namespaces.OpenId,
                Realm = realm,
                ReturnTo = QueryHelpers.AddQueryString(
                    uri: properties.Items[OpenIdAuthenticationConstants.Properties.ReturnTo],
                    name: OpenIdAuthenticationConstants.Parameters.State,
                    value: Options.StateDataFormat.Protect(properties))
            };

            if (Options.Attributes.Count != 0)
            {
                // openid.ns.ax (http://openid.net/srv/ax/1.0)
                message.SetParameter(
                    prefix: OpenIdAuthenticationConstants.Prefixes.Namespace,
                    name: OpenIdAuthenticationConstants.Aliases.Ax,
                    value: OpenIdAuthenticationConstants.Namespaces.Ax);

                // openid.ax.mode (fetch_request)
                message.SetParameter(
                    prefix: OpenIdAuthenticationConstants.Prefixes.Ax,
                    name: OpenIdAuthenticationConstants.Parameters.Mode,
                    value: OpenIdAuthenticationConstants.Modes.FetchRequest);

                foreach (var attribute in Options.Attributes)
                {
                    message.SetParameter(
                        prefix: OpenIdAuthenticationConstants.Prefixes.Ax,
                        name: $"{OpenIdAuthenticationConstants.Prefixes.Type}.{attribute.Key}",
                        value: attribute.Value);
                }

                // openid.ax.required
                message.SetParameter(
                    prefix: OpenIdAuthenticationConstants.Prefixes.Ax,
                    name: OpenIdAuthenticationConstants.Parameters.Required,
                    value: string.Join(",", Options.Attributes.Select(attribute => attribute.Key)));
            }

            var address = QueryHelpers.AddQueryString(Options.AuthenticationEndpoint, message.Parameters);

            Response.Redirect(address);
        }
        
        private new OpenIdAuthenticationEvents Events => (OpenIdAuthenticationEvents) base.Events;
    }
}
