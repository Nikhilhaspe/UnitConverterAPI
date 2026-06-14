namespace UnitConverterAPI.Services;

using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using UnitConverterAPI.Models;

public class ConversionMetadataService : IConversionMetadataService
{
    private readonly ILogger<ConversionMetadataService> _logger;
    public Dictionary<string, CategoryDefination> Categories {get; }

    public ConversionMetadataService(IWebHostEnvironment webHostEnvironment, ILogger<ConversionMetadataService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // set categories
        var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "data", "conversion-categories.json");
        
        if(!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Could not metdata file at location: {filePath}");
        }

        var json = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        Categories = JsonSerializer.Deserialize<Dictionary<string, CategoryDefination>>(json, options)!;
    }

    public IEnumerable<string> GetUnitsForCategory(string category)
    {
        var normalizedCategory = category.ToLowerInvariant();

        if(!Categories.TryGetValue(normalizedCategory, out var categoryDefination))
        {
            throw new KeyNotFoundException($"Conversion category {category} was not found.");
        }

        if(categoryDefination.Units == null || categoryDefination.Units.Count == 0)
            return ["celsius", "fahrenheit", "kelvin"];
        
        return categoryDefination.Units.Keys;
    }

    public double PerformConversion(ConversionRequest request)
    {
        var categoryKey = request.Category?.Trim().ToLowerInvariant() ?? string.Empty;
        var fromUnitKey = request.FromUnit?.Trim().ToLowerInvariant() ?? string.Empty;
        var toUnitKey = request.ToUnit?.Trim().ToLowerInvariant() ?? string.Empty;

        if(!Categories.TryGetValue(categoryKey, out var categoryDef))
        {
            throw new KeyNotFoundException($"Conversion category '{request.Category}' was not found.");
        }

        // For temperature
        if(categoryKey == "temperature")
        {
            return ConvertTemperature(fromUnitKey, toUnitKey, request.Value);
        }

        if(categoryDef == null || 
        !categoryDef.Units.TryGetValue(fromUnitKey, out var fromRate) ||
        !categoryDef.Units.TryGetValue(toUnitKey, out var toRate))
        {
            throw new ArgumentException($"Invalid input mappings provided for category '{request.Category}'.");
        }

        double valueInBaseUnit = request.Value * fromRate;
        return valueInBaseUnit / toRate;
    }

    public BatchConversionResponse PerformBatchConversion(BatchConversionRequest batchRequest)
    {
        var response = new BatchConversionResponse();

        foreach(var request in batchRequest.Conversions)
        {
            var resultItem = new BatchConversionResultItem { Request = request };

            try
            {
                resultItem.Result = PerformConversion(request);
                resultItem.Success = true;
            }
            catch(Exception ex)
            {
                resultItem.Success = false;
                resultItem.ErrorMessage = ex.Message;
            }

            response.Results.Add(resultItem);
        }

        return response;
    }

    // Private Methods
    private double ConvertTemperature(string from, string to, double value) =>
        from == to ? value : ConvertTo(to, ConvertToCelsius(from, value));

    private static double ConvertToCelsius(string from, double val) => from switch
    {
        "fahrenheit" => (val - 32) / 1.8,
        "kelvin"     => val - 273.15,
        _ => val
    };

    private static double ConvertTo(string to, double celsius) => to switch
    {
        "fahrenheit" => (celsius * 1.8) + 32,
        "kelvin"     => celsius + 273.15,
        _            => celsius
    };
}