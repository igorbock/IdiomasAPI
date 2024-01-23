namespace IdiomasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : Controller
{
    private readonly IService<GeminiPro, GeminiProResponse> _service;

    public ChatController(IService<GeminiPro, GeminiProResponse> service)
    {
        _service = service;
    }

    [HttpPost]
    [SwaggerOperation("Realizar chat com IA")]
    public async Task<GeminiProResponse> Post(GeminiPro request) => await _service.Get(request);
}
