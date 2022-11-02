using OYan.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables();

// Add services to the container.
// ×¢²áConsulRegistry£¨·þÎñ×¢²á£©
builder.Services.AddConsulRegistry(builder.Configuration);
builder.Services.AddControllers();
// Ìí¼Óswagger
builder.Services.AddSwagger(builder);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger(builder.Configuration);
//}

app.UseAuthorization();

app.MapControllers();

app.Run();