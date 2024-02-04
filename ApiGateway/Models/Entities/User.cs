using LoginProto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiGateway.Models.Entities;

public class User(string email, string password)
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Email { get; set; } = email;
    public string Password{ get; set; } = password;

    public static User FromLoginRequest(LoginRequest request) => new User(request.Email, request.Password);

    public RegistrationResponse ToRegistrationResponse() =>
        new RegistrationResponse { Email = this.Email, Success = true };
}