namespace UnitConverterAPI.Models.Common;

public class ApiResponse<T>
{
    public bool Success {get; set;}
    public string? Message {get; set;}
    public T? Data {get; set;}
    public DateTime Timestamp {get; set;} = DateTime.Now;

    public static ApiResponse<T> CreateSuccess(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }
}