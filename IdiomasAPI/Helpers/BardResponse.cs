namespace IdiomasAPI.Helpers;

public class BardResponse : IHttpResponseMethods<ChatResponse>
{
    public async Task<ChatResponse> GetHttpResponseTypeT(HttpResponseMessage httpResponse)
    {
        var responseJson = await httpResponse.Content.ReadAsStringAsync();
        var jObject = JObject.Parse(responseJson);
        var text = (string)jObject.SelectToken("candidates[0].content.parts[0].text")!;

        var retorno = new ChatResponse { Response = text };
        return retorno;
    }
}
