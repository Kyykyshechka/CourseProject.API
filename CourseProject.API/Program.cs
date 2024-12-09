using CourseProject.API.Controllers;
using CourseProject.BLL.Interfaces;
using CourseProject.BLL.Services;
using CourseProject.Core.Options;
using CourseProject.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CourseProjectDbContext>((provider, ctx) =>
{
    var options = provider.GetRequiredService<IOptions<DbOptions>>().Value;
    ctx.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CourseProjectAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
});

builder.Services.AddScoped<IIndicatorService, IndicatorService>();
builder.Services.AddScoped<IBackgroundImageService, BackgroundImageService>();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5500");
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowCredentials();
                      });

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<IndicatorHub>("/indicator");

app.MapControllers();

app.Run();
