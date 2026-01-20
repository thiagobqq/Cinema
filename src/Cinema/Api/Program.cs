using Auth.Infra.Seeder;
using Auth.WebApi;
using MediatR;
using Microsoft.OpenApi.Models;
using Movie.WebApi;
using Shared.Events;
using Tickets.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(MovieModule).Assembly)
    .AddApplicationPart(typeof(AuthModule).Assembly)
    .AddApplicationPart(typeof(TicketsModule).Assembly);
builder.Services.AddAuthModule(builder.Configuration);
builder.Services.AddMovieModule(builder.Configuration);
builder.Services.AddTicketsModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cinema API Modular", Version = "v1" });
 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
     options.AddPolicy("AllowFrontend", policy => 
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(Program).Assembly,
    typeof(MovieModule).Assembly,   
    typeof(AuthModule).Assembly,   
    typeof(TicketsModule).Assembly  
));

var app = builder.Build();

EventDispatcher.Configure(app.Services.GetRequiredService<IMediator>());

await RoleSeeder.SeedAsync(app.Services);
await UserSeeder.SeedAsync(app.Services);


if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();