using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiHome4.Data;
using WebApiHome4.DTOs;
using WebApiHome4.Mapping;
using WebApiHome4.Repositories;
using WebApiHome4.Results;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(value => value.Errors)
            .Select(error => error.ErrorMessage)
            .Where(message => !string.IsNullOrWhiteSpace(message));
        var result = ReturnResult<object>.Failure(string.Join(" ", errors));
        return new BadRequestObjectResult(result);
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("BookCatalog")
    ?? throw new InvalidOperationException("Connection string 'BookCatalog' is missing.");
builder.Services.AddDbContext<BookCatalogDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddAutoMapper(cfg => { }, typeof(BookMappingProfile));

var app = builder.Build();

app.UseExceptionHandler(handler => handler.Run(async context =>
{
    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("GlobalExceptionHandler");
    var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
    logger.LogError(exception, "Unhandled error while processing {Path}", context.Request.Path);
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    context.Response.ContentType = "application/json; charset=utf-8";
    await JsonSerializer.SerializeAsync(context.Response.Body,
        ReturnResult<object>.Failure("Произошла внутренняя ошибка. Повторите запрос позже."),
        cancellationToken: context.RequestAborted);
}));

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookCatalogDbContext>();
    db.Database.Migrate();
}

app.Run();
