using FundParser.Configuration;
using FundParser.DAL;
using Microsoft.EntityFrameworkCore;

var customAllowSpecificOrigins = "_customAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: customAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<FundParserDbContext>(options =>
{
    options.EnableSensitiveDataLogging();
});

builder.Services.ConfigureDependencyInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(customAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();