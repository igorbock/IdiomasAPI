namespace IdiomasLIB.Interfaces;

public interface IService<RequestType, ResponseType> : IDisposable
{
    Task<ResponseType> Get(RequestType requestEntity);
}
