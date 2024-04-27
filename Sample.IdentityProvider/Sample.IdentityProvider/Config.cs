using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Sample.IdentityProvider.Options;

namespace Sample.IdentityProvider;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        [ 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope(name: "Sample.Api", displayName: "Sample API")
        ];

    public static IEnumerable<Client> Clients(ApiClientOptions apiClient, WebClientOptions webClient) =>
        [
            new Client
            {
                ClientId = apiClient.ClientId,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret(apiClient.Secret.Sha256()) },
                AllowedScopes = { "Sample.Api" }
            },
            new Client
            {
                ClientId = webClient.ClientId,
                ClientSecrets = { new Secret(webClient.Secret.Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { $"{webClient.RedirectUrl}/signin-oidc" },
                PostLogoutRedirectUris = { $"{webClient.RedirectUrl}/signout-callback-oidc" },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "Sample.Api"
                }
            }
        ];
}