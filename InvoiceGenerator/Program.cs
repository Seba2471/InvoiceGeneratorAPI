using InvoiceGenerator;
using InvoiceGenerator.Persistance.EF;
using InvoiceGenerator.PersistanceEF.Repositories;
using InvoiceGenerator.Persistence;
using InvoiceGenerator.Repositories;
using InvoiceGenerator.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

var builder = WebApplication.CreateBuilder(args);

//Repository
builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(ITokenRepository<>), typeof(TokenRepository<>));
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

//PDF generator
builder.Services.AddRazorTemplating();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<IGeneratePdf, GeneratePdf>();

//AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Database connection
builder.Services
    .AddDbContext<InvoiceGeneratorContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("InvoiceGeneratorConnectionString")))
    .AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InvoiceGeneratorContext>();

//Authentication settings
var jsonWebTokensSettings = new JsonWebTokensSettings();
builder.Configuration.GetSection("JSONWebTokensSettings").Bind(jsonWebTokensSettings);
builder.Services.AddSingleton(jsonWebTokensSettings);

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // SignIn settings
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // User settings
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jsonWebTokensSettings.Audience,
            ValidIssuer = jsonWebTokensSettings.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jsonWebTokensSettings.AccessKey))
        };

        options.Events = new JwtBearerEvents()
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                if (context.AuthenticateFailure is not null)
                {
                    var tokenExpires = context.AuthenticateFailure.Message.Contains("Lifetime validation failed. The token is expired.");
                    if (tokenExpires)
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                }
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject("401 Not authorized");
                return context.Response.WriteAsync(result);
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject("403 Not authorized");
                return context.Response.WriteAsync(result);
            },
        };
    });

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
.WithExposedHeaders("Token-Expired"));




app.UseHttpsRedirection();


app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();


app.Run();