// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IDS4.AuthCenter
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("orderApi", "訂單服務")
                {
                    ApiSecrets ={ new Secret("orderApi secret".Sha256()) },
                    Scopes = { "orderApiScope" }
                },
                new ApiResource("productApi", "產品服務")
                {
                    ApiSecrets ={ new Secret("productApi secret".Sha256()) },
                    Scopes = { "productApiScope" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("orderApiScope"),
                new ApiScope("productApiScope"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "web client",
                    ClientName = "Web Client",

                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("web client secret".Sha256()) },

                    RedirectUris = { "https://localhost:5000/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5000/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5000/signout-callback-oidc" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "orderApiScope", "productApiScope"
                    },
                    AllowAccessTokensViaBrowser = true,

                    RequireConsent = true,  // 是否顯示同意界面
                    AllowRememberConsent = false, // 是否記得同意選項
                }
            };
    }
}