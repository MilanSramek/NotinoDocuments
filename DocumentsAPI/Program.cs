using Documents.Business;
using Documents.Repository.InMemory;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Formatters;

using Serilog;
using Serilog.Core;

using static System.Net.Mime.MediaTypeNames;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Logger logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    builder.Logging
        .ClearProviders()
        .AddSerilog(logger, true);

    builder.Services
        .AddInMemoryDocumentsRepository()
        .AddBusiness(builder.Configuration)
        .AddControllers(options =>
        {
            options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
        });

    WebApplication application = builder.Build();
    application.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = Text.Plain;

            IExceptionHandlerPathFeature? exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is ApplicationException applicationException)
                await context.Response.WriteAsync(applicationException.Message);
            else
                await context.Response.WriteAsync("An exception occured while processing the request.");
        });
    }); ;
    application.UseHsts();
    application.UseHttpLogging();
    application.UseHttpsRedirection();
    application.MapControllers();
    application.Run();
}
catch (Exception exception)
{
    logger.Fatal(exception, "Host terminated unexpectedly");
}
