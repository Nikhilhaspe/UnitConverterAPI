namespace UnitConverterAPI.Models;

public class CategoryDefination
{
    public string DisplayName {get; set;} = string.Empty;
    public string BaseUnit {get; set;} = string.Empty;
    public string? Type {get; set;}

    public Dictionary<string, double> Units {get; set;} = new Dictionary<string, double>();
}