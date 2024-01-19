namespace IdiomasAPI.Helpers;

public class QuestionResponse : IHttpResponseMethods<IQueryable<Question>>
{
    public async Task<IQueryable<Question>> GetHttpResponseTypeT(HttpResponseMessage httpResponse)
    {
        var responseJson = await httpResponse.Content.ReadAsStringAsync();
        var jObject = JObject.Parse(responseJson);
        var text = (string)jObject.SelectToken("candidates[0].content.parts[0].text")!;

        var retorno = new List<Question>();
        var questions = text.Split("\n").Where(a => a.Contains('?'));
        foreach (var item in questions)
        {
            var question = item.Split('.');
            retorno.Add(new Question(int.Parse(question[0]), question[1].Trim()));
        }

        return retorno.AsQueryable();
    }
}
