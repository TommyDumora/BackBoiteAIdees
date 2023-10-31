using BoiteAIdees.Context;
using BoiteAIdees.Services.BoiteAIdeesService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BoiteAIdeesContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BoiteAIdees")));
builder.Services.AddScoped<IBoiteAIdeesService, BoiteAIdeesService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
