using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Serilog;
using WebGLives.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(loggingBuilder =>
{
    var logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.Seq("http://localhost:5341")
        .CreateLogger();
    loggingBuilder.AddSerilog(logger, dispose: true);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins", policy => { policy.WithOrigins("http://localhost:3000");});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IZipService, ZipService>();
builder.Services.AddSingleton<IGamePagesRepository, GamePagesRepository>();

var app = builder.Build();
app.UseCors("MyAllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var contentTypeProvider = new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".unityweb"] = "application/octet-stream";
var staticFilesPath = Path.Combine(Path.GetTempPath(), "WebGLives");
var fileProvider = new PhysicalFileProvider(staticFilesPath);

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = fileProvider,
    ContentTypeProvider = contentTypeProvider
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = fileProvider,
});

app.UseAuthorization();

app.MapControllers();

app.Run();
