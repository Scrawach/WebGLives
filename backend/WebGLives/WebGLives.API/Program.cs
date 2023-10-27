using WebGLives.API.Extensions;

const string cors = "MyAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: cors, policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwtAuthentication();

builder.Services.AddWebGLives(builder);
builder.Services.AddConfiguredIdentity(builder);

var app = builder.Build();

app.UseCors(cors);
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