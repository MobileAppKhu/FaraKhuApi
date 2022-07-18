using Domain.Enum;

namespace Domain.BaseModels;

public class Error
{
    public string Message { get; set; }

    public ErrorType ErrorType { get; set; }
}