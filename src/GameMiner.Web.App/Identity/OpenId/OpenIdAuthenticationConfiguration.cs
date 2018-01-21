/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.IdentityModel.Protocols;

namespace GameMiner.Web.App.Identity.OpenId
{
    /// <summary>
    /// Represents an OpenID 2.0 configuration.
    /// </summary>
    public class OpenIdAuthenticationConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdAuthenticationConfiguration"/> class.
        /// </summary>
        public OpenIdAuthenticationConfiguration() { }

        /// <summary>
        /// Gets or sets the authentication endpoint address.
        /// </summary>
        public string AuthenticationEndpoint { get; set; }
    }
}
