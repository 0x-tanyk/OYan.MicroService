using Microsoft.EntityFrameworkCore;
using OYan.Core;
using OYan.EF.MySQL.Common;

namespace OYan.EF.WebAp
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options => options.AddPolicy("AllowAllMethods",
                builder => builder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .SetPreflightMaxAge(TimeSpan.FromSeconds(8 * 60 * 60))));
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwagger(builder);
            builder.Services.AddEndpointsApiExplorer();

            var connStr = builder.Configuration["ConnectStrings"];
            builder.Services.AddDbContext<OYanDbContext>(options => options.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger(builder.Configuration);

            app.MapControllers();

            app.Run();
        }
    }
}