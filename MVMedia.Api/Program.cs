using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVMedia.Api.DTOs.Mapping;
using MVMedia.Api.Identity;
using MVMedia.Api.Videos;
using MVMedia.Api.Repositories;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services;
using MVMedia.Api.Services.Interfaces;
using System.Text;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        //CONNECSTION STRING CONFIGURATION
        ///CONNECTION IN HOMOLOGATION
        var PostgreSqlConnection = builder.Configuration.GetConnectionString("QAConnection");
        ///CONNECTION IN PRD - RAILWAY
        //var PostgreSqlConnection = builder.Configuration.GetConnectionString("PRDConnection");
        //builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseNpgsql(PostgreSqlConnection));

        //DEPENDENCE INJECTION FRO REPOSITORIES AND SERVICES
        //REPOSITORIES
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IMediaRepository, MediaRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMediaFileRepository, MediaFileRepository>();
        builder.Services.AddScoped<IMediaListRepository, MediaListRepository>();
        builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

        //SERVICES
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IMediaSerivce, MediaService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthtenticate, AuthenticateService>();
        builder.Services.AddScoped<IMediaFileService, MediaFileService>();
        builder.Services.AddScoped<IMediaListService, MediaListService>();
        builder.Services.AddScoped<ICompanyService, CompanyService>();

        //ADD SERVICES AUTOMAPPER
        builder.Services.AddAutoMapper(cfg => cfg.AddProfile<EntitiesToDTOMappingProfile>());

        ////CONFIGURATION FOR FILE PATH UPLOAD
        //builder.Services.Configure<VideoSettings>(builder.Configuration.GetSection("VideoSettings"));


        //START configuration for JWT authentication

        //BUSCANDO DADOS DO appsettings.json
        var jwtSettings = builder.Configuration.GetSection("Jwt");


        // Add services to the container.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
                // Customização da resposta 401
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Usuário não autenticado" });
                        return context.Response.WriteAsync(result);
                    }
                };
            });


        // CORS para React + cookies
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ReactPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MVMedia API", Version = "v1" });

            //1. Define o esquema de segurança
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insita o token JWT no formato: Bearer {seu token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            //2. Aplica a exigência de segurança globalmente
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {

                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
            });
        });
        //END of JWT authentication configuration

        // Carregar VideoSettings do appsettings.json
        var videoSettings = builder.Configuration.GetSection("VideoSettings").Get<VideoSettings>();

        builder.Services.AddSingleton(videoSettings);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("AllowAll");

        app.MapControllers();

        app.Run();
    }
}