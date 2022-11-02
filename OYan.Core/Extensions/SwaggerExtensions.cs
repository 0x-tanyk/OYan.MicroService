using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OYan.Core
{
    /// <summary>
    /// Swagger扩展
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置信息</param>
        /// <returns></returns>
        public static void AddSwagger(this IServiceCollection services, WebApplicationBuilder app)
        {
            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            var configuration = app.Configuration;
            var apiVersions = configuration["Service:Version"].Split(',').ToList();
            services.AddSwaggerGen(options =>
            {
                foreach (var apiVersion in apiVersions)
                {
                    options.SwaggerDoc($"v{apiVersion}",
                        new OpenApiInfo
                        {
                            Title = $"{configuration["Service:Title"]} 版本{apiVersion}",
                            Version = $"v{apiVersion}",
                            Description = $"{configuration["Service:Description"]}",
                            Contact = new OpenApiContact
                            {
                                Name = configuration["Service:Contact:Name"],
                                Email = configuration["Service:Contact:Email"]
                            }
                        });
                }

                var xmlFiles = configuration["Service:XmlFiles"].Split(',').ToList();
                foreach (var xmlFile in xmlFiles)
                {
                    var xmlpath = Path.Combine(app.Environment.ContentRootPath, xmlFile);
                    options.IncludeXmlComments(xmlpath, true);
                }

                //设置bearer认证                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme(){
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },Array.Empty<string>() }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                        return false;
                    return true;
                });
            });
        }

        /// <summary>
        /// 服务swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration">配置信息</param>
        public static void UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var routeTemplate = $"doc/{configuration["Service:Name"]}" + "/{documentName}/swagger.json";
            app.UseSwagger(c =>
            {
                c.RouteTemplate = routeTemplate;
            });

            var apiVersions = configuration["Service:Version"].Split(',').ToList();
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersion in apiVersions)
                {
                    c.SwaggerEndpoint($"/doc/{configuration["Service:Name"]}/v{apiVersion}/swagger.json",
                    $"{configuration["Service:Name"]} v{apiVersion}");
                }
                //c.RoutePrefix = string.Empty; //swagger访问路由，为空则为 index.html， 不设计为 swagger/index.html
                //隐藏模型
                c.DefaultModelsExpandDepth(-1);
                //折叠文档
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
        }

        /// <summary>
        /// 网关swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration">配置信息</param>
        public static void UseGatewaySwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var routeTemplate = $"doc/{configuration["Service:Name"]}" + "/{documentName}/swagger.json";
            app.UseSwagger(c =>
            {
                c.RouteTemplate = routeTemplate;
            });
            var apiList = configuration["Swagger:ServiceDocNames"].Split(',').ToList();
            app.UseSwaggerUI(options =>
            {
                apiList.ForEach(apiItem =>
                {
                    //Api-版本
                    var api = apiItem.Split('-');
                    options.SwaggerEndpoint($"/doc/{api[0]}/{api[1]}/swagger.json", apiItem);
                });
                //options.RoutePrefix = string.Empty;
                //隐藏模型
                options.DefaultModelsExpandDepth(-1);
                //折叠文档
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
        }
    }
}
