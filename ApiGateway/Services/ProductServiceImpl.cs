using CommonProto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProductProto;

namespace ApiGateway.Services;

public class ProductServiceImpl : ProductService.ProductServiceBase
{
    private Channel _channel = new Channel("localhost:5053", ChannelCredentials.Insecure);

    public override async Task<ProductList> GetAll(Empty request, ServerCallContext context)
    {
        var client = new ProductService.ProductServiceClient(_channel);
        return await client.GetAllAsync(new Empty());
    }

    public override async Task<Product> GetById(MongoId request, ServerCallContext context)
    {
        var client = new ProductService.ProductServiceClient(_channel);
        return await client.GetByIdAsync(request);
    }
}