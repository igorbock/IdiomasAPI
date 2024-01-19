namespace IdiomasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : Controller
{
    private readonly IService<Image, ChatResponse> _imageService;

    public ImageController(IService<Image, ChatResponse> imageService)
    {
        _imageService = imageService;
    }

    [HttpPost]
    [SwaggerOperation("Leitura de imagem em string64")]
    public async Task<ChatResponse> ReadImage(Image image) => await _imageService.Get(image);
}
