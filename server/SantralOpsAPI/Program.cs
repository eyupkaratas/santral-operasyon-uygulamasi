using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SantralOpsAPI.Persistence;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<SantralOpsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
      };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "SantralOpsAPI", Version = "v1" });
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Description = "JWT Authorization header. Örnek: 'Bearer {token}'",
    Type = SecuritySchemeType.ApiKey,
    In = ParameterLocation.Header,
    Name = "Authorization",
    Scheme = "Bearer"
  });
  c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                Name = "Bearer",
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Seed
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  var context = services.GetRequiredService<SantralOpsDbContext>();
  await context.Database.MigrateAsync();
  await DataSeeder.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

var isDocker = Environment.GetEnvironmentVariable("IS_DOCKER");

if (!string.IsNullOrEmpty(isDocker))
{
  app.Urls.Add("http://0.0.0.0:5049");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();