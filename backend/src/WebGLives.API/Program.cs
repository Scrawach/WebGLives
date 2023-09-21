using Microsoft.AspNetCore.Identity;
using WebGLives.API.Extensions;
using WebGLives.DataAccess;

const string cors = "MyAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: cors, policy => { policy.WithOrigins("http://localhost:3000").AllowAnyMethod();});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWebGLives(builder);
builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<GamesDbContext>();

var app = builder.Build();
app.UseCors(cors);
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpLogging();

app.UseWebGLivesStorage();

app.UseAuthorization();

app.MapControllers();

app.Run();
