using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using OYan.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("Config/appsettings.json")
    .AddJsonFile($"Config/appsettings.{builder.Environment.EnvironmentName}.json")
    .AddJsonFile("Config/ocelot.json")
    .AddEnvironmentVariables();

builder.Services.AddCors(options => options.AddPolicy("AllowAllMethods",
    builder => builder.AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .SetPreflightMaxAge(TimeSpan.FromSeconds(8 * 60 * 60))));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger(builder);
builder.Services.AddEndpointsApiExplorer();
//add ocelot
builder.Services.AddOcelot().AddConsul();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseGatewaySwagger(builder.Configuration);
//}

//app.UseHttpsRedirection();

app.UseOcelot();

//app.UseAuthorization(); 
app.MapControllers().RequireCors("AllowAllMethods");
app.Run();