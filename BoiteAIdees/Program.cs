using BoiteAIdees.Context;
using BoiteAIdees.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "La boîte à idées");
    });
}

app.UseCors("VueJsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "La boîte à idées", Version = "v1" });
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        c.EnableAnnotations();
    });
    services.AddDbContext<BoiteAIdeesContext>(options => options.UseSqlServer(configuration.GetConnectionString("BoiteAIdees")));
    services.AddScoped<IdeasService>();
    services.AddScoped<CategoriesService>();
    services.AddScoped<UsersService>();
    services.AddScoped<UserLikedIdeasService>();

    services.AddCors(options =>
    {
        options.AddPolicy("VueJsPolicy", policy =>
        {
            policy.
                WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
}
