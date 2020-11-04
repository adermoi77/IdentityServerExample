// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Ipd
{
    using IdentityServer4;
    using IdentityServer4.Models;

    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Config" />.
    /// </summary>
    public static class Config
    {
        #region Properties

        /// <summary>
        /// Gets the ApiScopes.
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
            };

        /// <summary>
        /// Gets the Clients.
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // 客户端凭据
                new Client
                {
                    ClientId = "credentialsClient",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1" }
                } ,

                //wpf ResourcesOwnerPasswordCredentials
                new Client
                {
                    ClientId="wpf Client",
                    ClientName="wpf Credentials client", 
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                    ClientSecrets={new Secret("wpfSecret".Sha256()) },
                    AllowedScopes = {"scope1",IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Email }
                }

            };

        /// <summary>
        /// Gets the IdentityResources.
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        #endregion
    }
}
