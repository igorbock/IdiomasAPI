var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IHttpResponseMethods<IQueryable<Question>>, QuestionResponse>();
builder.Services.AddScoped<ICRUDService<Glossary>, GlossaryService>();
builder.Services.AddScoped<ICRUDService<Question>, QuestionService>();
builder.Services.AddScoped<ICRUDService<Answer>, AnswerService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.EnableAnnotations();
});

builder.Services.AddDbContext<IdiomasContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpClient("GoogleAPI", a =>
{
    a.BaseAddress = new Uri("https://generativelanguage.googleapis.com");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
