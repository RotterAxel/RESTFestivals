// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Test;

namespace IDP.Quickstart
{
    public static class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "fec0a4d6-5830-4eb8-8024-272bd5d6d2bb",
                Username = "Organizer",
                Password = "password",
                Claims = new List<Claim>
                {
                    new Claim("given_name", "Kevin"),
                    new Claim("family_name", "Dockx"),
                    new Claim("role", "Festival_Organizer"),
                }
            },
            new TestUser
            {
                SubjectId = "c3b7f625-c07f-4d7d-9be1-ddff8ff93b4d",
                Username = "Admin",
                Password = "password",
                Claims = new List<Claim>
                {
                    new Claim("given_name", "Sven"),
                    new Claim("family_name", "Vercauteren"),
                    new Claim("role", "Festival_Admin"),
                }
            },
            new TestUser
            {
                SubjectId = "77910d08-75da-4ea5-97ac-e25173f8d510",
                Username = "Normal",
                Password = "password",
                Claims = new List<Claim>
                {
                    new Claim("given_name", "Normal"),
                    new Claim("family_name", "User")
                }
            }
        };
    }
}