using Reddit.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddAutoMapper();
builder.Services.ConfigIdentityService();
builder.Services.AddBussinessService();
builder.Services.ConfigureSwagger();

#pragma warning disable CS8604 // Possible null reference argument.
builder.Services.AddJWTAuthentication(builder.Configuration["Jwt:Key"], builder.Configuration["Jwt:Issuer"]);
#pragma warning restore CS8604 // Possible null reference argument.

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("http://localhost:3000", "http://localhost:4200");
}));

builder.Services.AddSignalR();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<StartupBase>>());
app.UseForwardedHeaders();
app.Run();
