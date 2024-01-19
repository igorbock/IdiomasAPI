namespace IdiomasLIB.Interfaces;

public interface IService<RequestType, ResponseType>
{
    Task<ResponseType> Get(RequestType requestEntity);
}
