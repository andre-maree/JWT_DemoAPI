using DemoAPIDataAccess;
using MemLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
string jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });

builder.Services.AddScoped<ICoinStatsService, CoinStatsService>();
builder.Services.AddScoped<IBankHolidaysService, BankHolidaysService>();
builder.Services.AddScoped<IBankHolidaysDA, BankHolidaysDA>();
builder.Services.AddScoped<IMenuDA, MenuDA>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<ILoginDA, LoginDA>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ICoinStatsService, CoinStatsService>()
        .ConfigureHttpClient((serviceProvider, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("BaseUrlCoinStats").Get<string>());
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", builder.Configuration.GetSection("CoinStatsServiceKey").Get<string>());

        });

builder.Services.AddHttpClient<IBankHolidaysService, BankHolidaysService>()
        .ConfigureHttpClient((serviceProvider, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("BankHolidaysServiceURL").Get<string>());

        });

builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
        });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
