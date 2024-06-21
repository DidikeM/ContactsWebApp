using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ContactsWebApp.Client.Utils
{
    public class AuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient) : AuthenticationStateProvider
    {
        private readonly AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorageService.GetItemAsStringAsync("token");

            if (string.IsNullOrEmpty(token))
            {
                return _anonymous;
            }

            var email = await localStorageService.GetItemAsync<string>("email");
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Email, email!)], "JwtAuth"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return new AuthenticationState(claimsPrincipal);
        }

        public void NotifyUserAuthentication(string email)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Email, email)], "JwtAuth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
