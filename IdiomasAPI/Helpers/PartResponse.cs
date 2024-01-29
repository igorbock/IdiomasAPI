namespace IdiomasAPI.Helpers;

public class PartResponse : IHttpResponseMethods<IQueryable<Part>>
{
    public async Task<IQueryable<Part>> GetHttpResponseTypeT(HttpResponseMessage httpResponse)
    { 
        var responseJSON = await httpResponse.Content.ReadAsStringAsync();
        var retorno = JsonSerializer.Deserialize<IQueryable<Part>>(responseJSON);
        return retorno!;
    }
}
