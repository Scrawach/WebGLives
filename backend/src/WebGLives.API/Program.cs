using Microsoft.EntityFrameworkCore;
using Serilog;
using WebGLives.API.Extensions;
using WebGLives.API.Services;
using WebGLives.DataAccess;
using WebGLives.DataAccess.Repositories;

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

builder.Services.AddDbContext<GamesDbContext>(options =>
{
    options.UseNpgsql
    (
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DataAccessMappingProfile>();
});

builder.Services.AddSingleton<IRandomService, RandomService>();
builder.Services.AddSingleton<IZipService, ZipService>();
builder.Services.AddSingleton<IFilesService, FilesService>();
builder.Services.AddSingleton<IGamesService, GamesService>();
builder.Services.AddScoped<IGamesRepository, GamesRepository>();

var app = builder.Build();
app.UseCors("MyAllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseGameStorage();

app.UseAuthorization();

app.MapControllers();

app.Run();
