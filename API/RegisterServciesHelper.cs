using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Services.Custom;
using BL.GeneralService.CMS;
using BL.Mapper;
using BL.Mapper.Base;
using BL.Services;
using BL.Services.Custom;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Generic;
using DAL.Contracts.UnitOfWork;
using DAL.Repositories.Generic;
using DAL.UnitOfWork;
using Domains.Entities.Identity;
using Domains.Helpers;
using Domains.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;



namespace API
{
    public class RegisterServicesHelper
    {
        public static void RegisteredServices(WebApplicationBuilder builder )
        {
     
            #region Connection To SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
             builder.Configuration.GetConnectionString("DefaultConnection"))); 
            #endregion

            #region Identity
            builder.Services.AddIdentity<ApplicationUser, Role>(option =>
            {
                // Password settings.
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;

                // User settings.
                option.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                option.User.RequireUniqueEmail = true;
                option.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders(); 
            #endregion

            #region JWT Authentication
            var jwtSettings = new JwtSettings();
            builder.Configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuers = new[] { jwtSettings.Issuer },
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidAudience = jwtSettings.Audience,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidateLifetime = jwtSettings.ValidateLifeTime,
                };
            });
            #endregion

            #region Configure Serilog
            // Configure Serilog for logging
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
            connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            tableName: "Log",
            autoCreateSqlTable: true)
           .CreateLogger();
            // Register Serilog logger
            builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

            #endregion

            #region Localization
            builder.Services.AddControllersWithViews();
            builder.Services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });
            #endregion

            #region Swagger Gn
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cinema Ticket Booking System Project", Version = "v1" });
                c.EnableAnnotations();

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
          {
            {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
            }
         });
            });

            #endregion


            // Register Auto Mapper
            //builder.Services.AddAutoMapper(typeof(Program)); // Assuming 'Program' contains AutoMapper profiles
            //builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


            // With this corrected line:
            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));
            // line Cima
            //builder.Services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));


            // Register repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IBaseMapper), typeof(BaseMapper));

            // CMS
            //builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            //builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            builder.Services.AddScoped<IUserTokenService, UserTokenService>();
            builder.Services.AddScoped<IFileUploadService, FileUploadService>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<IImageProcessingService, ImageProcessingService>();
            //builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            //builder.Services.AddScoped<IUserProfileService, UserProfileService>();

            // Project Services
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();



            // Register localization and set the resources folder path
            //builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // add memory cache
            builder.Services.AddMemoryCache();

            //  Swagger Configuration
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "Book Review Platform API",
            //        Version = "v1",
            //        Description = "API for managing Book Review Platform services."
            //    });

            //    // JWT Bearer token authentication
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        In = ParameterLocation.Header,
            //        Description = "Please enter 'Bearer' [space] and then your token",
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey,
            //        BearerFormat = "JWT",
            //        Scheme = "Bearer"
            //    });

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //     {
            //      new OpenApiSecurityScheme
            //     {
            //    Reference = new OpenApiReference
            //    {
            //        Type = ReferenceType.SecurityScheme,
            //        Id = "Bearer"
            //    }
            //    },
            //new string[] {}
            //    }
            //    });
            //  });

            builder.Services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = false; // Changed to false to prevent BREACH/CRIME attacks
            });

            // Configure Gzip compression options
            builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Optimal;
            });

            // Configure CORS to allow requests from any origin
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });


        }
    }
}
