/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace GameMiner.Web.App.Identity.OpenId
{
    public static class OpenIdAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="OpenIdAuthenticationHandler{TOptions}"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables OpenID 2.0 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOpenId(this AuthenticationBuilder builder)
        {
            return builder.AddOpenId(OpenIdAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="OpenIdAuthenticationHandler{TOptions}"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables OpenID 2.0 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOpenId(
            this AuthenticationBuilder builder,
            Action<OpenIdAuthenticationOptions> configuration)
        {
            return builder.AddOpenId(OpenIdAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="OpenIdAuthenticationHandler{TOptions}"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables OpenID 2.0 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOpenId(
            this AuthenticationBuilder builder, string scheme,
            Action<OpenIdAuthenticationOptions> configuration)
        {
            return builder.AddOpenId(scheme, OpenIdAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="OpenIdAuthenticationHandler{TOptions}"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables OpenID 2.0 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="name">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOpenId(
            this AuthenticationBuilder builder,
            string scheme, string name,
            Action<OpenIdAuthenticationOptions> configuration)
        {
            return builder.AddOpenId<OpenIdAuthenticationOptions, OpenIdAuthenticationHandler<OpenIdAuthenticationOptions>>(scheme, name, configuration);
        }

        /// <summary>
        /// Adds <see cref="OpenIdAuthenticationHandler{TOptions}"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables OpenID 2.0 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="name">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOpenId<TOptions, THandler>(
            this AuthenticationBuilder builder,
            string scheme, string name,
            Action<TOptions> configuration)
            where TOptions : OpenIdAuthenticationOptions, new()
            where THandler : OpenIdAuthenticationHandler<TOptions>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(scheme))
            {
                throw new ArgumentException("The scheme cannot be null or empty.", nameof(scheme));
            }

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<TOptions>,
                                            OpenIdAuthenticationInitializer<TOptions, THandler>>());

            return builder.AddRemoteScheme<TOptions, THandler>(scheme, name, configuration);
        }
    }
}
