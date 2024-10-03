using Application;
using Application.Common.Exceptions;
using Domain.Environment;
using Infra;
using Infra.Database;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var envVariables = builder.Configuration.Get<EnvVariables>();
builder.Services.AddDbContextPool<Context>(opt => opt.UseSqlServer(envVariables.SQLConnection));

builder.Services.AddApplication();
builder.Services.AddInfra();

builder.Services.AddExceptionHandler(options =>
{
    options.ExceptionHandler = GlobalExceptionHandler.Handle;
    options.AllowStatusCode404Response = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(option => option
    .SetIsOriginAllowed(_ => true)
    .AllowAnyHeader()
    .WithMethods("POST", "PUT", "DELETE", "GET")
    .AllowCredentials()
);

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var logger = app.Services.GetService<ILogger<Program>>();
    logger.LogInformation("### Running migration.");

    for (int i = 0; i < 3; i++)
    {
        try
        {
            var db = scope.ServiceProvider.GetService<Infra.Database.Context>();
            db.Database.Migrate();
            break;
        }
        catch (Exception ex)
        {
            Console.Write("### Error in update database with migration:");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException);

            Thread.Sleep(10000);

        }
    }

    logger.LogInformation("### Migration applied successful.");
}

app.Run();
