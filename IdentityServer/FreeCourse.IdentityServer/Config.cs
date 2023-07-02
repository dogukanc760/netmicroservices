// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;

using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},
            new ApiResource("photo_stock_catalog"){Scopes={"photo_stock_fullpermission"}},
            new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
            new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},

            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)

        };


        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles", DisplayName="Roles", Description="Users Roles", UserClaims= new[]{"role"}}
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","Catalog API allows full access"),
                new ApiScope("photo_stock_fullpermission","Photo Stock API allows full access"),
                new ApiScope("basket_fullpermission","Basket API allows full access"),
                new ApiScope("discount_fullpermission","Discount API allows full access"),
                //new ApiScope("discount_read","Discount API allows get"),
                //new ApiScope("discount_write","Discount API allows write"),


                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };
        // aşağıda kullanıcı bazlı ve platform bazlı hatta platform içerisinde ki modül veya komponent bazlı bile yetkilendirme yapabiliyoruz.
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                
                // client credentials (user bazlı değil uygulama bazlı izin verme, örneğin hepsiburadaya üye olmasan bile stokları görebilmen gibi)
                new Client
                {
                    ClientName="Aspnet Mvc Web App",
                    ClientId="WebMvcClient",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "catalog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },
                 new Client
                {
                     // Resource Owner Password Credentials yani kullanıcı bazlı yetki ve izinlere tabi olarak bir şeyleri görmemizi yapmamızı sağlayan kod blokları
                    ClientName="Aspnet Mvc Web App For User",
                    ClientId="WebMvcClientForUser",
                    AllowOfflineAccess=true,
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={
                         "basket_fullpermission", "discount_fullpermission",
                         IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess,IdentityServerConstants.LocalApi.ScopeName,
                         "roles"
                     },
                    AccessTokenLifetime=1*60*60,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenUsage=TokenUsage.ReUse
                },
                new Client
                {
                    ClientName="NextJS Web App",
                    ClientId="WebNextJSClient",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "catalog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client
                {
                    ClientName="React-Native Mobile App",
                    ClientId="MobileReactNativeApp",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "catalog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },
            };
    }
}