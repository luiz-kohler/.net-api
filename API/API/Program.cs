using Infra;
using Application;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Domain.Environment;

var builder = WebApplication.CreateBuilder(args);

var envVariables = builder.Configuration.Get<EnvVariables>();
builder.Services.AddDbContextPool<Context>(opt => opt.UseSqlServer(envVariables.SQLConnection));

builder.Services.AddApplication();
builder.Services.AddInfra();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
