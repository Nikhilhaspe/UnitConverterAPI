namespace UnitConverterAPI.Models;

public class BatchConversionRequest
{
    public List<ConversionRequest> Conversions {get; set;} = [];
}

public class BatchConversionResultItem
{
    public bool Success { get; set; }
    public ConversionRequest Request { get; set; } = null!;
    public double? Result { get; set; }
    public string? ErrorMessage { get; set; }
}

public class BatchConversionResponse
{
    public List<BatchConversionResultItem> Results { get; set; } = [];
}