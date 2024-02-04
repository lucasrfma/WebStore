using ApiGateway.Models.Entities;
using ApiGateway.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LoginProto;
using MongoDB.Driver;

namespace ApiGateway.Services;

public class LoginServiceImpl(MongoCollections mongoCollections, TokenService tokenService) : LoginService.LoginServiceBase
{
    private readonly IMongoCollection<User> _usersCollection = mongoCollections.UsersCollection;

    public override async Task<RegistrationResponse> Register(LoginRequest request, ServerCallContext context)
    {
        User user = User.FromLoginRequest(request);
        await _usersCollection.InsertOneAsync(user);
        return user.ToRegistrationResponse();
    }

    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var user = await _usersCollection.FindAsync(u =>
            u.Email.Equals(request.Email) && u.Password.Equals(request.Password));
        if (user.Any())
        {
            var (token, expires) = tokenService.GenerateToken(request);
            return new LoginResponse {
                Success = true,
                Token = token,
                Expiration = expires
            };        
        }
        return new LoginResponse
        {
            Success = false,
            Token = "",
            Expiration = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
        };
    }
}