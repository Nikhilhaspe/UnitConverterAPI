namespace UnitConverterAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using UnitConverterAPI.Models;
using UnitConverterAPI.Services;

// length, temperature & weight

[ApiController]
[Route("api/v1/[controller]")]
public class ConversionController : ControllerBase
{
    private readonly IConversionMetadataService _metadata;

    public ConversionController(IConversionMetadataService metadata)
    {
        _metadata = metadata ?? throw new NullReferenceException();
    }

    [HttpGet("categories")]
    public IActionResult GetConversionCategories()
    {
        var categories = _metadata.Categories;
        return Ok(categories);
    }

    [HttpGet("{category}/units")]
    public IActionResult GetConversionCategoryUnits(string category)
    {
        var unitNames = _metadata.GetUnitsForCategory(category);
        return Ok(unitNames);
    }

    [HttpPost]
    public IActionResult DoConversion([FromBody] ConversionRequest request)
    {
        var calculatedResult = _metadata.PerformConversion(request);
        return Ok(calculatedResult);
    }

    [HttpPost("batch")]
    public IActionResult DoBatchConversion([FromBody] BatchConversionRequest batchRequest)
    {
        if(batchRequest?.Conversions == null || batchRequest.Conversions.Count == 0)
        {
            throw new ArgumentException("Batch conversion list cannot be empty.");
        }

        var batchResult = _metadata.PerformBatchConversion(batchRequest);
        return Ok(batchResult);
    }
}