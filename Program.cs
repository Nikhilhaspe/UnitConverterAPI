using Serilog;
using UnitConverterAPI.Filters;
using UnitConverterAPI.Middlewares;
using UnitConverterAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Serilog for logging
Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
builder.Host.UseSerilog();

// DI & Services
// Filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseWrapperFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// DI
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddSingleton<IConversionMetadataService, ConversionMetadataService>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Unit Converter API v1");
    });
}

// custom global error handling middleware
app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapControllers();

app.Run();