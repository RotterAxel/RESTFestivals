// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace MoMi.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Your role(s)", new []{"role"})
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("festivalsapi",
                    new[] { "role" })
            };
        
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "Postman",
                    AllowOfflineAccess = true,
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "festivalsapi"
                    },
                    RedirectUris = new []
                    {
                        "https://www.getpostman.com/oauth2/callback"
                    },
                    Enabled = true,
                    ClientId = "832afa32-cabe-40a0-8909-2241cd85e47d",
                    ClientSecrets = new[]
                    {
                        new Secret("NotASecret".Sha256()), 
                    },
                    PostLogoutRedirectUris = new []
                    {
                        "http://localhost:5002/signout-callback-oidc"
                    },
                    ClientUri = null,
                    AllowedGrantTypes = new[]
                    {
                        GrantType.ResourceOwnerPassword
                    }
                } 
            };
        
    }
}