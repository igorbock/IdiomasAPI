namespace IdiomasLIB.Interfaces;

public interface IHttpResponseMethods<TypeT>
{
    Task<TypeT> GetHttpResponseTypeT(HttpResponseMessage httpResponse);
}
