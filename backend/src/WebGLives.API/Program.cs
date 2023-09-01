using Microsoft.EntityFrameworkCore;
using Serilog;
using WebGLives.API.Extensions;
using WebGLives.API.Services;
using WebGLives.API.Services.Abstract;
using WebGLives.BusinessLogic.Services;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.DataAccess;
using WebGLives.DataAccess.Repositories;

const string cors = "MyAllowSpecificOrigins";

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
    options.AddPolicy(name: cors, policy => { policy.WithOrigins("http://localhost:3000");});
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

builder.Services.AddWebGLives();

var app = builder.Build();
app.UseCors(cors);

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
