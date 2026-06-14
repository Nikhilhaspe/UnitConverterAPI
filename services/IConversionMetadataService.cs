namespace UnitConverterAPI.Services;

using UnitConverterAPI.Models;
using UnitConverterAPI.Models.Common;

public interface IConversionMetadataService
{
    Dictionary<string, CategoryDefination> Categories {get; }
    IEnumerable<string> GetUnitsForCategory(string category);
    double PerformConversion(ConversionRequest request);
    BatchConversionResponse PerformBatchConversion(BatchConversionRequest request);
}