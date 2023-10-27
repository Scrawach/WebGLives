using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebGLives.API;
using WebGLives.API.Contracts.Auth;
using WebGLives.API.Contracts.Users;

namespace WebGLives.Tests.Integration.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> CreateGame(this HttpClient client, string accessToken)
    {
        client.DefaultRequestHeaders.Authorization = RequestBuilder.BearerAuthenticationHeader(accessToken);
        var createdResponse = await client.PostAsync(ApiRouting.Games, null);
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        return createdResponse;
    }
    
    public static async Task<AuthenticatedResponse> CreateTestUser(this HttpClient client)
    {
        const string login = "test";
        const string password = "test123";
        return await CreateAuthenticatedUser(client, login, password);
    }
    
    public static async Task<AuthenticatedResponse> CreateAuthenticatedUser(this HttpClient client, string login, string password)
    {
        await CreateUser(client, login, password);
        var tokensResponse = await client.PostAsync(ApiRouting.Tokens, RequestBuilder.CreateLoginRequest(login, password));
        tokensResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = await tokensResponse.Content.ReadFromJsonAsync<AuthenticatedResponse>();
        tokens.Should().NotBeNull();
        
        return tokens!;
    }
    
    public static async Task<UserResponse> CreateUser(this HttpClient client, string login, string password)
    {
        var userCreatedResponse = await client.PostAsync(ApiRouting.Users, RequestBuilder.CreateUserRequest(login, password));
        userCreatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await userCreatedResponse.Content.ReadFromJsonAsync<UserResponse>();
        return user!;
    }
}