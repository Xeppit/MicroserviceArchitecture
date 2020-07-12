// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;

namespace MicroserviceArchitecture.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.OpenId(),
                       //new IdentityResources.Profile(),
                       //overridden below
                       new CustomIdentityResourcesProfile(), // New Line
                       new IdentityResources.Email(),
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("gatewayapi", "The Gateway Api", new[] { JwtClaimTypes.Role }), // Modified Line
                new ApiScope("microserviceapi1", "Microservice Api 1"), // New Line
                new ApiScope("microserviceapi2", "Microservice Api 2"), // New Line
                new ApiScope("microserviceapi3", "Microservice Api 3")  // New Line
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "blazor",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedCorsOrigins = {"https://localhost:5001"},
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris = {"https://localhost:5001/authentication/login-callback"},
                    PostLogoutRedirectUris = {"https://localhost:5001/"},
                    AllowedScopes = {"openid", "profile", "email", "gatewayapi"}
                },
            };
    }
}