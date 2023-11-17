using WebGLives.API.Extensions;
using WebGLives.API.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwtAuthentication();

builder.Services.AddWebGLives(builder);
builder.Services.AddConfiguredIdentity(builder);

var app = builder.Build();

app.UseCors(CorsOptionsSetup.CorsName);
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpLogging();

app.UseWebGLivesStorage();

app.MapControllers();

app.Run();

public partial class Program { }