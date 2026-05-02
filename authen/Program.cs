using authen.common.BaseClass;
using authen.common.data.Models;
using authen.common.Helpers;
using authen.Database.Mongodb.Collection;
using authen.system.web.MenuAndRole;
using authen.system.web.Controller;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SpaServices.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ── Module controller registry ──────────────────────────────────────────────
// Each sub-project exposes a static ListController with its ControllerAppModel list.
// Add each module here so Program.cs knows all routes, roles, and public actions.
ListController.list = [

    ..SystemListController.list_controller

    ];
foreach (var controller in SystemListController.list_controller)
{
    ListController.list_public.AddRange(controller.list_controller_action_public);
    ListController.list_not_public.AddRange(controller.list_controller_action_publicNonLogin);
}

   
// Future: allModuleControllers.AddRange(HrListController.list_controller);
builder.Services.AddSingleton(ListController.list_public);
builder.Services.AddControllers()
    .AddNewtonsoftJson();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var jwtSettings = builder.Configuration.GetSection("AppSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    // Use default authen method Jwt
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Configuring how to treat people without right
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    }); 
});

// solve for development environment
builder.Services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

builder.Services.AddScoped<MongoDBContext>(s => {


    var appSettings = appSettingsSection.Get<AppSettings>();
    var httpContext = s.GetService<IHttpContextAccessor>()?.HttpContext;
    var host = httpContext?.Request.Host.Host;
    if (string.IsNullOrEmpty(host)) host = "localhost";
    var factory = s.GetRequiredService<IMongoClientFactory>();
    string databaseName = "";
    if (host.Contains("localhost"))
    {
        databaseName = appSettings.mongodb_database;
    }
    else
    {
        databaseName = appSettings.default_database;
        // Ví dụ: tenant1.domain.com → tenant1
        //databaseName = host.Split('.')[0];
    }

    return factory.GetClientDatabase(databaseName);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger(option =>
//    {
//        // documentName = v1
//        option.RouteTemplate = "{documentName}/swagger.json";
//    });
//    app.UseSwaggerUI(option =>
//    {
//        option.SwaggerEndpoint("_api/v1/swagger.json", "AspNetAngularSamePort API");
//        option.RoutePrefix = "swagger";
//    });
//}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}.ctr/{action=Index}/{id?}");
//app.MapFallbackToFile("index.html");

//app.UseSpa(spa =>
//{
//    spa.Options.SourcePath = "AuthECClient";
//    if (app.Environment.IsDevelopment())
//    {
//        //spa.UseAngularCliServer(npmScript: "start");
//        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
//    }
//});

app.Run();
